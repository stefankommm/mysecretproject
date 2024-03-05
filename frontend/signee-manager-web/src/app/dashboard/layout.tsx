'use client'

import { useRouter } from "next/navigation";
import React, { Fragment, useEffect } from "react";
import { isUserLoggedIn } from "@/utils/authUtils";

const DashboardLayout = ({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) => {
    const router = useRouter();

    // Check if user is authenticated (TODO validate token expiration)
    useEffect(() => {
        const checkAuth = async () => {
            const isAuthenticated = await isUserLoggedIn();
            if (!isAuthenticated) router.push("/");
        };

        checkAuth();
    }, [router]);

    return (
        <Fragment>
            {children}
        </Fragment>
    );
}

export default DashboardLayout;
