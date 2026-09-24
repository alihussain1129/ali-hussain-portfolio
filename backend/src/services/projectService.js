import * as repo from '../repositories/projectRepository.js';
import { HttpError } from '../middleware/errorHandler.js';
import { slugify } from '../utils/urls.js';

export const listPublic = () => repo.listProjects({ includeUnpublished: false });
export const listAll = () => repo.listProjects({ includeUnpublished: true });

export async function getPublicBySlug(slug) {
  const project = await repo.findBySlug(slug);
  if (!project || !project.isPublished) throw new HttpError(404, 'Project not found');
  return project;
}

export async function create(input) {
  const slug = input.slug || slugify(input.title);
  if (!slug) throw new HttpError(400, 'Could not create a slug from the title');
  if (await repo.findBySlug(slug)) {
    throw new HttpError(409, 'A project with this slug already exists');
  }
  const id = await repo.insertProject({ ...input, slug });
  return repo.findById(id);
}

export async function update(id, input) {
  const existing = await repo.findById(id);
  if (!existing) throw new HttpError(404, 'Project not found');

  const slug = input.slug || existing.slug;
  const clash = await repo.findBySlug(slug);
  if (clash && clash.id !== id) {
    throw new HttpError(409, 'A project with this slug already exists');
  }

  await repo.updateProject(id, { ...input, slug });
  return repo.findById(id);
}

export async function remove(id) {
  const deleted = await repo.deleteProject(id);
  if (!deleted) throw new HttpError(404, 'Project not found');
}