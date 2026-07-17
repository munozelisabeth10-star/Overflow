'use client';

import {Button} from "@heroui/button";
import {handleError, successToast} from "@/lib/util";
import {testAuth} from "@/app/session/auth-actions";

export default function AuthTestButton() {
    const onClick = async () => {
        const {data,error}= await testAuth();
        if(error)handleError(error);
        if(data) successToast(data);
    }
    
    return (
        <Button
            color='success'
            onPress={onClick}
        >
            Test Auth
        </Button>
    );
}
