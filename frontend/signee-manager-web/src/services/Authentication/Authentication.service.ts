import { ILoginRequest, IUserData } from "../../types/authTypes";
import CoreAPIService from '@external/src/services/CoreAPI/CoreAPI.service';

class AuthenticationService {

    public login = async (
        url: string,
        data: ILoginRequest
    ): Promise<{ status: number; data: { message: string; userData: IUserData }}> => {
        const response = await CoreAPIService.post(url + '/api/user/login', data);
        return {
            status: response.status,
            data: response.data
        };
    }

}

const authenticationService = new AuthenticationService();
export default authenticationService;