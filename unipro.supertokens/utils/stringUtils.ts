import { Problem } from "../models/problem";

export const isNullOrEmpty = (
  stringToCheck: string | any | null,
  returningProblemObject: Problem,
  problemMessage?: string): boolean => {
    
  if (typeof stringToCheck !== "string" || stringToCheck.length === 0) {
    returningProblemObject.type = 'https://httpstatuses.com/400';
    returningProblemObject.title = 'Bad request';
    returningProblemObject.status = 400;
    returningProblemObject.detail = problemMessage;
    
    return true;
  }
  
  return false;
}