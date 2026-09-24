import { getPool, sql } from '../config/db.js';

export async function findByUsername(username) {
  const pool = await getPool();
  const result = await pool
    .request()
    .input('username', sql.NVarChar(50), username)
    .query(`SELECT Id AS id, Username AS username, PasswordHash AS passwordHash
            FROM dbo.AdminUsers WHERE Username = @username`);
  return result.recordset[0] ?? null;
}

export async function createAdmin(username, passwordHash) {
  const pool = await getPool();
  await pool
    .request()
    .input('username', sql.NVarChar(50), username)
    .input('hash', sql.NVarChar(100), passwordHash)
    .query('INSERT INTO dbo.AdminUsers (Username, PasswordHash) VALUES (@username, @hash)');
}