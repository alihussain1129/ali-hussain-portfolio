import { Router } from 'express';
import rateLimit from 'express-rate-limit';
import { requireAdmin } from '../middleware/auth.js';
import { asyncHandler } from '../middleware/errorHandler.js';
import { loginSchema } from '../validators/projectValidator.js';
import { login } from '../services/authService.js';

const router = Router();

const loginLimiter = rateLimit({
  windowMs: 15 * 60 * 1000,
  limit: 10,
  standardHeaders: true,
  legacyHeaders: false,
  message: { error: 'Too many login attempts. Try again later.' },
});

router.post('/login', loginLimiter, asyncHandler(async (req, res) => {
  const { username, password } = loginSchema.parse(req.body);
  res.json(await login(username, password));
}));

router.get('/me', requireAdmin, (req, res) => {
  res.json({ username: req.admin.username });
});

export default router;