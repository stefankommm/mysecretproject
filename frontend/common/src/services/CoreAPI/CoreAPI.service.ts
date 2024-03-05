import axios, { AxiosResponse } from 'axios';
import { StringMapping } from "../../utils/stringUtils";

class CoreAPIService {

    private async getAccessToken(): Promise<string> {
        return localStorage.getItem('accessToken') || '';
    }    

    /**
     * Makes a GET request to the specified URL.
     * @param {string} url - URL of the endpoint.
     * @param {any} params - Query parameters for the request.
     * @param {any} headers - Headers for the request.
     * @param {boolean} useAuth - Append JWT access token to the request headers if exists in localStorage. By default it's true.
     * @returns {Promise<AxiosResponse<any>>} - Promise resolving to the AxiosResponse.
     */
    public async get(
        url: string,
        params = {},
        headers: any = {},
        useAuth: boolean = false
    ): Promise<AxiosResponse<any>> {
        headers.Authorization = useAuth  ? ('Bearer ' + this.getAccessToken) : '';
        return axios.request({
            method: 'get',
            url,
            headers,
            params
        });
    }

    /**
     * Makes a POST request with JSON data in the request body.
     * @param {string} url - URL of the endpoint.
     * @param {StringMapping<any>} data - JSON data to send in the request body.
     * @param {any} headers - Headers for the request.
     * @param {boolean} useAuth - Append JWT access token to the request headers if exists in localStorage. By default it's true.
     * @returns {Promise<AxiosResponse<any>>} - Promise resolving to the AxiosResponse.
     */
    public async post(
        url: string,
        data: StringMapping<any>,
        headers: any = {},
        useAuth: boolean = false
    ): Promise<AxiosResponse<any>> {
        headers.Authorization = useAuth  ? ('Bearer ' + this.getAccessToken) : '';
        return axios.request({
            method: 'post',
            url,
            headers,
            data
        });
    }

    /**
     * Makes a POST request with form data in the request body. It's suitable for sending 
     * form data encoded as application/x-www-form-urlencoded or multipart/form-data
     * @param {string} url - URL of the endpoint.
     * @param {StringMapping<any> | null} formData - Form data to send in the request body.
     * @param {any} headers - Headers for the request.
     * @param {boolean} useAuth - Append JWT access token to the request headers if exists in localStorage. By default it's true.
     * @returns {Promise<AxiosResponse<any>>} - Promise resolving to the AxiosResponse.
     */
    public async postFormData(
        url: string,
        formData: StringMapping<any> | null,
        headers: any = {},
        useAuth: boolean = false
    ): Promise<AxiosResponse<any>> {
        headers.Authorization = useAuth  ? ('Bearer ' + this.getAccessToken) : '';
        return axios.post(url, formData, { headers });
    }

    /**
     * Makes a PUT request to the specified URL with the provided data.
     * @param {string} url - URL of the endpoint.
     * @param {StringMapping<any>} data - The data to send in the request body.
     * @param {StringMapping<string>} headers - Headers for the request.
     * @param {boolean} useAuth - Append JWT access token to the request headers if exists in localStorage. By default it's true.
     * @returns {Promise<AxiosResponse<any>>} - Promise resolving to the AxiosResponse.
     */
    public async put(
        url: string,
        data: StringMapping<any>,
        headers: StringMapping<string> = {},
        useAuth: boolean = false
    ): Promise<AxiosResponse<any>> {
        headers.Authorization = useAuth  ? ('Bearer ' + this.getAccessToken) : '';
        return axios.request({
            method: 'put',
            url,
            headers,
            data
        });
    }

    /**
     * Makes a PATCH request to the specified URL with the provided data.
     * @param {string} url - URL of the endpoint.
     * @param {StringMapping<any>} data - The data to send in the request body.
     * @param {any} headers - Headers for the request.
     * @param {boolean} useAuth - Append JWT access token to the request headers if exists in localStorage. By default it's true.
     * @returns {Promise<AxiosResponse<any>>} - Promise resolving to the AxiosResponse.
     */
    public async patch(
        url: string,
        data: StringMapping<any>,
        headers: any = {},
        useAuth: boolean = false
    ): Promise<AxiosResponse<any>> {
        headers.Authorization = useAuth  ? ('Bearer ' + this.getAccessToken) : '';
        return axios.request({
            method: 'patch',
            url,
            headers,
            data
        });
    }

    /**
     * Makes a DELETE request to the specified URL with the provided data.
     * @param {string} url - URL of the endpoint.
     * @param {StringMapping<any>} data - The data to send in the request body.
     * @param {any} headers - Headers for the request.
     * @param {boolean} useAuth - Append JWT access token to the request headers if exists in localStorage. By default it's true.
     * @returns {Promise<AxiosResponse<any>>} - Promise resolving to the AxiosResponse.
     */
    public async delete(
        url: string,
        data: StringMapping<any>,
        headers: any = {},
        useAuth: boolean = false
    ): Promise<AxiosResponse<any>> {
        headers.Authorization = useAuth  ? ('Bearer ' + this.getAccessToken) : '';
        return axios.request({
            method: 'delete',
            url,
            headers,
            data
        });
    }
}

export default new CoreAPIService();