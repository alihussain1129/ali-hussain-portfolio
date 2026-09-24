import { getPool, closePool } from '../src/config/db.js';

try {
  const pool = await getPool();

  const who = await pool
    .request()
    .query('SELECT DB_NAME() AS db, SUSER_SNAME() AS login_name');
  console.log('Connected.', who.recordset[0]);

  const tables = await pool
    .request()
    .query(
      "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME"
    );
  console.log(
    'Tables:',
    tables.recordset.map((r) => r.TABLE_NAME).join(', ') || '(none yet)'
  );
} catch (err) {
  console.error('Connection failed:', err.code, '-', err.message);
  process.exitCode = 1;
} finally {
  await closePool();
}