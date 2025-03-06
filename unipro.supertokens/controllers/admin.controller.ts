// import EmailPassword from "supertokens-node/recipe/emailpassword";
import { Querier } from "supertokens-node/lib/build/querier";
import { Problem } from "../models/problem";
import { query } from "../services/database";
import { isNullOrEmpty } from "../utils/stringUtils";
import NormalisedURLPath from "supertokens-node/lib/build/normalisedURLPath";

export const createUser = async (req: any, res: any) => {
  try {
    const { email, password, firstName, lastName, patronymic } = req.body;

    let badRequest: Problem = {};

    if (isNullOrEmpty(email, badRequest, "User creation failed. Invalid email.") ||
      isNullOrEmpty(password, badRequest, "User creation failed. Invalid password.") ||
      isNullOrEmpty(firstName, badRequest, "User creation failed. Invalid first name.") ||
      isNullOrEmpty(lastName, badRequest, "User creation failed. Invalid last name.") ||
      isNullOrEmpty(patronymic, badRequest, "User creation failed. Invalid patronymic.")) {
      return res.status(400).json(badRequest);
    }
    
    const querier = Querier.getNewInstanceOrThrowError(undefined);
    const signUpResponse = await querier.sendPostRequest(new NormalisedURLPath("/recipe/signup"), {
      email,
      password
    // @ts-ignore
    }, {});

    if (signUpResponse.status !== 'OK') {
      badRequest = {
        type: 'https://httpstatuses.com/400',
        title: 'Bad request',
        status: 400,
        detail: 'User creation failed.'
      }

      return res.status(400).json(badRequest);
    }
    
    console.log(signUpResponse);

    const queryText = "INSERT INTO users (app_id, user_id, first_name, last_name, patronymic, created_at) VALUES ($1, $2, $3, $4, $5, $6);";
    const params = ["public", signUpResponse.user.id, firstName, lastName, patronymic, new Date().toISOString()];

    const result = await query(queryText, params);

    if (result.rowCount && result.rowCount > 0) {
      res.json({
        status: "OK",
        user: signUpResponse.user,
      });
    } else {
      badRequest = {
        type: 'https://httpstatuses.com/500',
        title: 'Internal Server Error',
        status: 500,
        detail: 'User was successfully created, but not populated with personal data.'
      }

      return res.status(400).json(badRequest);
    }
  } catch (error: any) {
    res.status(500).json({
        type: 'https://httpstatuses.com/500',
        title: 'Internal Server Error',
        status: 500,
        detail: error.message,
    });
  }
}
