/* eslint-disable @typescript-eslint/no-unused-vars */
import NextAuth, {DefaultSession} from 'next-auth';
import {JWT} from 'next-auth/jwt';
import {number, string} from "zod";
//los imoprts se ponen para que en auth.ts no de errores con el async jwt y el async session

declare module 'next-auth' {
    interface Session {
        user: {
            id: string
            displayName: string
            reputation: number
        } & DefaultUser
        accessToken: string;
    }
    
    interface User {
        username: string;
        displayName: string;
        reputation: number;
    }
}

declare module 'next-auth/jwt' {
    interface JWT {
        accessToken: string;
        refreshToken: string;
        accessTokenExpires: number;
        error?: string;

        user: {
            id: string;
            displayName: string;
            reputation: number;
        }
    }
}