import { Router } from 'express';
import { requireAdmin } from '../middleware/auth.js';
import { asyncHandler } from '../middleware/errorHandler.js';
import { projectSchema, idParamSchema } from '../validators/projectValidator.js';
import * as service from '../services/projectService.js';

const router = Router();

// Public
router.get('/', asyncHandler(async (req, res) => {
  res.json(await service.listPublic());
}));

// Admin only (this route must stay above '/:slug')
router.get('/admin/all', requireAdmin, asyncHandler(async (req, res) => {
  res.json(await service.listAll());
}));

router.get('/:slug', asyncHandler(async (req, res) => {
  res.json(await service.getPublicBySlug(req.params.slug));
}));

router.post('/', requireAdmin, asyncHandler(async (req, res) => {
  const input = projectSchema.parse(req.body);
  res.status(201).json(await service.create(input));
}));

router.put('/:id', requireAdmin, asyncHandler(async (req, res) => {
  const { id } = idParamSchema.parse(req.params);
  const input = projectSchema.parse(req.body);
  res.json(await service.update(id, input));
}));

router.delete('/:id', requireAdmin, asyncHandler(async (req, res) => {
  const { id } = idParamSchema.parse(req.params);
  await service.remove(id);
  res.status(204).end();
}));

export default router;