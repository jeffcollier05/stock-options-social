/**
 * Represents a view model for handling errors.
 */
export class ErrorViewModel {
    /**
     * The main error message.
     */
    message: string = '';
  
    /**
     * Additional details or information about the error.
     */
    details: string = '';
  
    /**
     * Creates an instance of ErrorViewModel.
     */
    constructor(message: string, details: string) {
      this.message = message;
      this.details = details;
    }
  }
  