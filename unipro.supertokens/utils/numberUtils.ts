import { Problem } from "../models/problem";

export const isNull = (
  stringNumberToCheck: string | any | null,
  returningProblemObject: Problem,
  problemMessage?: string): boolean => {

  if (typeof stringNumberToCheck !== "number") {
    returningProblemObject.type = 'https://httpstatuses.com/400';
    returningProblemObject.title = 'Bad request';
    returningProblemObject.status = 400;
    returningProblemObject.detail = problemMessage;

    return true;
  }

  return false;
}