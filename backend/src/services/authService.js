import bcrypt from 'bcryptjs';
import jwt from 'jsonwebtoken';
import { env } from '../config/env.js';
import { findByUsername } from '../repositories/adminRepository.js';
import { HttpError } from '../middleware/errorHandler.js';

// Compared when the username doesn't exist, so timing doesn't reveal valid usernames.
const DUMMY_HASH = bcrypt.hashSync('not-the-real-password', 12);

export async function login(username, password) {
  const admin = await findByUsername(username);
  const valid = await bcrypt.compare(password, admin ? admin.passwordHash : DUMMY_HASH);

  if (!admin || !valid) throw new HttpError(401, 'Invalid username or password');

  const token = jwt.sign(
    { sub: String(admin.id), username: admin.username },
    env.JWT_SECRET,
    { algorithm: 'HS256', expiresIn: env.JWT_EXPIRES_IN }
  );
  return { token, username: admin.username };
}