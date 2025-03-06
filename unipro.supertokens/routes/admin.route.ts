import express from "express";
import { createUser } from "../controllers/admin.controller";
import { SessionRequest } from "supertokens-node/framework/express";

const router = express.Router();

const isAdmin = (req: SessionRequest, res: any, next: any) => {
  next();
} 

router.post('/admin/users', isAdmin, createUser);

export default router;