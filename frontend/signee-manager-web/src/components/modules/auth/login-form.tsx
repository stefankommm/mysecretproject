'use client'

import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from "@/components/ui/form"
import {Input} from "@/components/ui/input"
import {Button} from "@/components/ui/button"
import { useToast } from "@/components/ui/use-toast"
import {z} from "zod"
import {zodResolver} from "@hookform/resolvers/zod";
import {useForm} from "react-hook-form";
import {useCallback} from "react";
import { ILoginRequest } from "@/types/authTypes";
import { useRouter } from 'next/navigation'
import authenticationService from "@/services/Authentication/Authentication.service";
import { useTranslation } from "react-i18next"

const formSchema = z.object({
    email: z.string(),
    password: z.string(),
})

interface IProps {
    url: string
}

const LoginForm = (props: IProps) => {
    const {url} = props;
    const { t } = useTranslation();
    const { toast } = useToast();
    const router = useRouter();

    const form = useForm<z.infer<typeof formSchema>>({
        resolver: zodResolver(formSchema),
        defaultValues: {
            email: "",
            password: "",
        },
    })

    const login = useCallback(async(loginRequest: ILoginRequest): Promise<boolean> => {
        let success = true;
        try {
            const resp = await authenticationService.login(url, loginRequest!);

            if (resp.status === 200) {
                localStorage.setItem("accessToken", resp.data.userData.token);
                toast({ title: resp.data.message });
                router.push("/dashboard");
            }
        } catch (e) {
            const err = e as any;
            if (err?.response?.data?.message) {
                toast({
                    title: err.response.data.message,
                    variant: "destructive"
                })
                success = false;
            }
        }
        return success;
    }, [url, toast, router]);

    const handleSubmit = useCallback(
        async (e: React.FormEvent<HTMLFormElement>) => {
            e.preventDefault(); // Prevent default form submission
            const loginRequest: ILoginRequest = form.getValues();
            await login(loginRequest);
        },
        [login, form]
    );

    return (
        <Form {...form}>
            <form onSubmit={handleSubmit}>
                <FormField
                    control={form.control}
                    name="email"
                    render={({field}) => (
                        <FormItem className="mb-1">
                            <FormLabel>{t("auth.email")}</FormLabel>
                            <FormControl>
                                <Input placeholder="youremail@mail.com" {...field} />
                            </FormControl>
                        </FormItem>
                    )}
                />
                <FormField
                    control={form.control}
                    name="password"
                    render={({field}) => (
                        <FormItem className="mb-6">
                            <FormLabel>{t("auth.password")}</FormLabel>
                            <FormControl>
                                <Input placeholder={t("auth.password")} type="password" {...field} />
                            </FormControl>
                        </FormItem>
                    )}
                />
                <Button className="w-full mb-2">
                    {t("auth.loginAction")}
                </Button>
            </form>
        </Form>
    )
}

export default LoginForm;