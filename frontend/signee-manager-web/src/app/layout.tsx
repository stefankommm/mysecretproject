"use client" // Force whole App to be client side rendered

import { Inter } from "next/font/google";
import "./globals.css";
import React from "react";
import { Toaster } from "@/components/ui/toaster";
import { I18nextProvider } from 'react-i18next';
import i18n from '../../i18n';

const inter = Inter({ subsets: ["latin"] });

const RootLayout = ({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) => {
  return (
    <I18nextProvider i18n={i18n}>
      <html lang="en">
          <body className={inter.className}>
            <main>{children}</main>
            <Toaster />
          </body>
      </html> 
    </I18nextProvider>
  );
}

export default RootLayout;
