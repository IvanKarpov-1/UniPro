import { Querier } from "supertokens-node/lib/build/querier";
import { Problem } from "../models/problem";
import { query } from "../services/database";
import { isNullOrEmpty } from "../utils/stringUtils";
import { isNull } from "../utils/numberUtils";
import NormalisedURLPath from "supertokens-node/lib/build/normalisedURLPath";

export const createUser = async (req: any, res: any) => {
  try {
    const {
      email,
      password,
      firstName,
      lastName,
      patronymic,
      role,
      universityId = null,
      academicId = null,
      departmentId = null,
      studentGroupId = null
    } = req.body;

    let badRequest: Problem = {};

    if (isNullOrEmpty(email, badRequest, "User creation failed. Invalid email.") ||
      isNullOrEmpty(password, badRequest, "User creation failed. Invalid password.") ||
      isNullOrEmpty(firstName, badRequest, "User creation failed. Invalid first name.") ||
      isNullOrEmpty(lastName, badRequest, "User creation failed. Invalid last name.") ||
      isNullOrEmpty(patronymic, badRequest, "User creation failed. Invalid patronymic.") ||
      isNullOrEmpty(role, badRequest, "User creation failed. Invalid role.") ||
      (role !== 'admin' && isNull(universityId, badRequest, "User creation failed. Invalid university ID.")) ||
      (role !== 'admin' && isNull(academicId, badRequest, "User creation failed. Invalid academic ID.")) ||
      (role !== 'admin' && isNull(departmentId, badRequest, "User creation failed. Invalid department ID."))) {
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
    
    const addUserRoleResponse = await querier.sendPutRequest(new NormalisedURLPath("/recipe/user/role"), {
      userId: signUpResponse.user.id,
      role: role,
      // @ts-ignore
    }, {}, {});
    
    if (addUserRoleResponse.status !== "OK") {
      badRequest = {
        type: 'https://httpstatuses.com/400',
        title: 'Bad request',
        status: 400,
        detail: 'User creation failed. Incorrect user role.'
      }
      
      await querier.sendPostRequest(new NormalisedURLPath("/user/remove"), {
        userId: signUpResponse.user.id,
        // @ts-ignore
      }, {});

      return res.status(400).json(badRequest);
    }
    
    const addUserQueryText = "INSERT INTO users (app_id, user_id, first_name, last_name, patronymic, created_at) VALUES ($1, $2, $3, $4, $5, $6);";
    const addUserParams = ["public", signUpResponse.user.id, firstName, lastName, patronymic, new Date().toISOString()];
    let result = await query(addUserQueryText, addUserParams);
    
    if (role === "student") {
      const addStudentInfoQueryText = "INSERT INTO student_infos (student_id, university_id, academic_id, department_id, student_group_id) VALUES ($1, $2, $3, $4, $5);"
      const addStudentInfoParams = [signUpResponse.user.id, universityId, academicId, departmentId, studentGroupId];
      result = await query(addStudentInfoQueryText, addStudentInfoParams);
    } else if (role === "teacher") {
      const addTeacherInfoQueryText = "INSERT INTO teacher_infos (teacher_id, university_id, academic_id, department_id) VALUES ($1, $2, $3, $4);"
      const addTeacherInfoParams = [signUpResponse.user.id, universityId, academicId, departmentId];
      result = await query(addTeacherInfoQueryText, addTeacherInfoParams);
    }
    
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
