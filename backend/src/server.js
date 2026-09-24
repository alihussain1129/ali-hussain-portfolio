import { env } from './config/env.js';
import { getPool, closePool } from './config/db.js';
import app from './app.js';

try {
  await getPool();
  console.log('Database connected');
} catch (err) {
  console.error('Database connection failed:', err.code, '-', err.message);
  process.exit(1);
}

const server = app.listen(env.PORT, () => {
  console.log(`API running on http://localhost:${env.PORT}`);
});

async function shutdown() {
  server.close(async () => {
    await closePool();
    process.exit(0);
  });
}
process.on('SIGINT', shutdown);
process.on('SIGTERM', shutdown);