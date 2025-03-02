import SuperTokens from "supertokens-node";
import EmailPassword from "supertokens-node/recipe/emailpassword";
import Session from "supertokens-node/recipe/session";
import { UserContext } from "supertokens-node/types";
import { query } from "./database";

interface Input {
  email: string;
  password: string;
  session: Session.SessionContainer | undefined;
  shouldTryLinkingWithSessionUser: boolean | undefined;
  tenantId: string;
  userContext: UserContext;
} 

const customSignUp = async function (originalImplementation: EmailPassword.RecipeInterface, input: Input) {
  // First we call the original implementation of signUp.
  let response = await originalImplementation.signUp(input);

  // Post sign up response, we check if it was successful
  if (response.status === "OK" && response.user.loginMethods.length === 1 && input.session === undefined) {
    /**
     *
     * response.user contains the following info:
     * - emails
     * - id
     * - timeJoined
     * - tenantIds
     * - phone numbers
     * - third party login info
     * - all the login methods associated with this user.
     * - information about if the user's email is verified or not.
     *
     */
    
    let firstName = "";
    let lastName = "";
    let patronymic = "";
    let avatar = "";
    const date = new Date().toISOString();
    const createdAt = date;
    const updatedAt = date;
    
    const request = SuperTokens.getRequestFromUserContext(input.userContext);
    
    if (request !== undefined) {      
      firstName = request.getHeaderValue("firstName") ?? "undefined";
      lastName = request.getHeaderValue("lastName") ?? "undefined";
      patronymic = request.getHeaderValue("patronymic") ?? "undefined";
      avatar = request.getHeaderValue("avatar") ?? "undefined";
    } else {      
      /**
       * This is possible if the function is triggered from the user management dashboard
       *
       * In this case set a reasonable default value to use
       */
      firstName = "Іван";
      lastName = "Карпов";
      patronymic = "Борисович";
      avatar = "";
    }
    
    const queryText = "INSERT INTO users (app_id, user_id, first_name, last_name, patronymic, avatar, created_at, updated_at) VALUES ($1, $2, $3, $4, $5, $6, $7, $8);";
    const params = ["public", response.user.id, firstName, lastName, patronymic, avatar, createdAt, updatedAt];

    const result = await query(queryText, params);
    
    if (result.rowCount === 1) {
      
    } else {
      
    }
  }
  return response;
}

export default customSignUp;