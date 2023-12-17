/**
 * Represents the result of JWT authentication, including the token, authentication result, and potential errors.
 */
export class JwtAuth {
    /**
     * The JWT token obtained after successful authentication.
     */
    token: string = '';
  
    /**
     * The result of JWT authentication, indicating whether it was successful.
     */
    result: boolean = false;
  
    /**
     * Any errors or additional information related to JWT authentication.
     */
    errors: any;
  }
  