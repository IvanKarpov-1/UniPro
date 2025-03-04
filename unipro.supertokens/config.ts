import EmailPassword from "supertokens-node/recipe/emailpassword";
import Session from "supertokens-node/recipe/session";
import Dashboard from "supertokens-node/recipe/dashboard";
import UserRoles from "supertokens-node/recipe/userroles";
import { TypeInput } from "supertokens-node/types";
import customSignUp from "./hooks";

export function getApiDomain() {
  const apiPort = process.env.APP_API_PORT || 3001;
  const apiUrl = process.env.APP_API_URL || `http://localhost:${apiPort}`;
  return apiUrl;
}

export function getWebsiteDomain() {
  const websitePort = process.env.APP_WEBSITE_PORT || 3000;
  const websiteUrl = process.env.APP_WEBSITE_URL || `http://localhost:${websitePort}`;
  return websiteUrl;
}

export const SuperTokensConfig: TypeInput = {
  supertokens: {    
    // this is the location of the SuperTokens core.
    connectionURI: process.env.SUPERTOKENS_CORE ?? "http://localhost:3567",
    apiKey: process.env.SUPERTOKENS_API_KEY,
  },
  appInfo: {
    appName: "UniPro",
    apiDomain: getApiDomain(),
    websiteDomain: getWebsiteDomain(),
    apiBasePath: '/api/auth'
  },
  // recipeList contains all the modules that you want to
  // use from SuperTokens. See the full list here: https://supertokens.com/docs/guides
  recipeList: [
    EmailPassword.init({
      override: {
        functions: originalImplementation => {
          return {
            ...originalImplementation,
            signUp: input => customSignUp(originalImplementation, input)
          }
        }
      }
    }), 
    Session.init({
      useDynamicAccessTokenSigningKey: false,
    }), 
    Dashboard.init(),
    UserRoles.init(),],
};