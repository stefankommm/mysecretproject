'use client'

import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import LoginForm from "@/components/modules/auth/login-form"
import RegisterForm from "@/components/modules/auth/register-form"
import { useRouter } from "next/navigation"
import { useEffect } from "react"
import { isUserLoggedIn } from "@/utils/authUtils"
import { useTranslation } from "react-i18next"

interface IProps {
    url: string
}

const UserAuth = (props: IProps) => {
    const {url} = props;
    const { t } = useTranslation();
    const router = useRouter();

    // Check if user is authenticated (TODO validate token expiration)
    useEffect(() => {
        const checkAuth = async () => {
            const isAuthenticated = await isUserLoggedIn();
            if (isAuthenticated) router.push("/dashboard");
        };

        checkAuth();
    }, [router]);

    return (
        <Tabs defaultValue="login">
            <TabsList className="w-full flex-row mb-2">
                <TabsTrigger value="login" className="grow">{t("auth.login")}</TabsTrigger>
                <TabsTrigger value="register" className="grow">{t("auth.register")}</TabsTrigger>
            </TabsList>
            <TabsContent value="login">
                <LoginForm url={url}/>
            </TabsContent>
            <TabsContent value="register">
                <RegisterForm url={url}/>
            </TabsContent>
        </Tabs>
    )
}

export default UserAuth;