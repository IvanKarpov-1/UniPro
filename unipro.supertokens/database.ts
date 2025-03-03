import pg from "pg";
import { Pool } from "pg";

const pool = new Pool({
  connectionString: process.env.POSTGRES_CONNECTION_STRING,
});

export const query = (text: string, params?: any[]): Promise<pg.QueryResult> => pool.query(text, params);