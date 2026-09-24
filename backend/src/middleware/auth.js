import jwt from 'jsonwebtoken';
import { env } from '../config/env.js';
import { HttpError } from './errorHandler.js';

export function requireAdmin(req, res, next) {
  const header = req.headers.authorization || '';
  const [scheme, token] = header.split(' ');

  if (scheme !== 'Bearer' || !token) {
    return next(new HttpError(401, 'Authentication required'));
  }

  try {
    req.admin = jwt.verify(token, env.JWT_SECRET, { algorithms: ['HS256'] });
    next();
  } catch {
    next(new HttpError(401, 'Invalid or expired token'));
  }
}