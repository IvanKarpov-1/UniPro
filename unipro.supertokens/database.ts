import pg from "pg";
import { Pool } from "pg";

const pool = new Pool({
  host: process.env.POSTGRES_HOST,
  user: process.env.POSTGRES_USER,
  password: process.env.POSTGRES_PASSWORD,
  database: process.env.POSTGRES_DB,
  port: Number.parseInt(process.env.POSTGRES_PORT ?? '5432'),
  idleTimeoutMillis: 30000
});

export const query = (text: string, params?: any[]): Promise<pg.QueryResult> => pool.query(text, params);