import sql from 'mssql';
import { env } from './env.js';

const config = {
  server: env.DB_SERVER,
  port: env.DB_PORT,
  database: env.DB_NAME,
  user: env.DB_USER,
  password: env.DB_PASSWORD,
  options: {
    encrypt: env.DB_ENCRYPT,
    trustServerCertificate: env.DB_TRUST_CERT,
  },
  pool: { max: 10, min: 0, idleTimeoutMillis: 30000 },
  connectionTimeout: 15000,
  requestTimeout: 15000,
};

let poolPromise;

export function getPool() {
  if (!poolPromise) {
    const pool = new sql.ConnectionPool(config);
    pool.on('error', (err) => console.error('SQL pool error:', err.message));
    poolPromise = pool.connect().catch((err) => {
      poolPromise = undefined; // allow a retry on the next call
      throw err;
    });
  }
  return poolPromise;
}

export async function closePool() {
  if (!poolPromise) return;
  const pool = await poolPromise;
  poolPromise = undefined;
  await pool.close();
}

export { sql };
