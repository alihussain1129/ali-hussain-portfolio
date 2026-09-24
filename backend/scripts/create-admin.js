import bcrypt from 'bcryptjs';
import { getPool, closePool } from '../src/config/db.js';
import { findByUsername, createAdmin } from '../src/repositories/adminRepository.js';

const [username, password] = process.argv.slice(2);

if (!username || !password) {
  console.error('Usage: npm run create-admin -- <username> "<password>"');
  process.exit(1);
}
if (password.length < 12) {
  console.error('Password must be at least 12 characters.');
  process.exit(1);
}

try {
  await getPool();
  if (await findByUsername(username)) {
    console.error(`Admin "${username}" already exists.`);
    process.exitCode = 1;
  } else {
    const hash = await bcrypt.hash(password, 12);
    await createAdmin(username, hash);
    console.log(`Admin "${username}" created.`);
  }
} catch (err) {
  console.error('Failed:', err.code, '-', err.message);
  process.exitCode = 1;
} finally {
  await closePool();
}