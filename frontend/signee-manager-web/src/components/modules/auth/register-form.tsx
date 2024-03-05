'use client'

import {
    Form,
    FormControl,
    FormDescription,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from "@/components/ui/form"
import { Input } from "@/components/ui/input"
import { Button } from "@/components/ui/button"
import { Checkbox } from "@/components/ui/checkbox";
import { z } from "zod"
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { useCallback } from "react";
import Link from "next/link";
import { useTranslation, Trans } from "react-i18next"

interface IProps {
    url: string
}

const RegisterForm = (props: IProps) => {
    const { url } = props;
    const { t } = useTranslation();

    const formSchema = z.object({
        username: z.string().min(4),
        email: z.string().email({message: t("auth.emailValidation")}),
        password: z.string().min(6),
        terms: z.boolean().refine((value) => value, {
            message: t("auth.termsValidation"),
        })
    })

    const form = useForm<z.infer<typeof formSchema>>({
        resolver: zodResolver(formSchema),
        defaultValues: {
            username: "",
            email: "",
            password: "",
            terms: undefined
        },
    })

    const handleSubmit = useCallback(
        async (e: React.FormEvent<HTMLFormElement>) => {
            e.preventDefault(); // Prevent default form submission
            // Additional logic for handling registration form submission
        },
        []
    );

    // TODO implement register function

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
                            <FormMessage/>
                        </FormItem>
                    )}
                />
                <FormField
                    control={form.control}
                    name="password"
                    render={({field}) => (
                        <FormItem className="mb-4">
                            <FormLabel>{t("auth.password")}</FormLabel>
                            <FormControl>
                                <Input type="password" placeholder={t("auth.password")} {...field} />
                            </FormControl>
                            <FormMessage/>
                        </FormItem>
                    )}
                />
                <FormField
                    control={form.control}
                    name="username"
                    render={({field}) => (
                        <FormItem className="mb-4">
                            <FormLabel>{t("auth.userName")}</FormLabel>
                            <FormControl>
                                <Input placeholder={t("auth.accountName")} {...field} />
                            </FormControl>
                            <FormMessage/>
                        </FormItem>
                    )}
                />
                <FormField
                    control={form.control}
                    name="terms"
                    render={({field}) => (
                        <FormItem className="mb-6">
                            <div className="flex row items-center justify-start">
                                <FormControl>
                                    <Checkbox
                                        checked={field.value}
                                        onCheckedChange={field.onChange}
                                    />
                                </FormControl>
                                <FormDescription className="pl-2">
                                    <Trans i18nKey="auth.termsAndConditions">
                                        <Link href="/" className="underline underline-offset-4 hover:text-primary">
                                        </Link> 
                                        <Link href="/" className="underline underline-offset-4 hover:text-primary">
                                        </Link>
                                    </Trans>
                                </FormDescription>
                            </div>
                            <FormMessage/>
                        </FormItem>
                    )}
                />
                <Button className="w-full mb-2">
                    Register
                </Button>
            </form>
        </Form>
    )
}

export default RegisterForm;