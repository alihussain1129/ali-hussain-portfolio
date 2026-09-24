import { z } from 'zod';
import { isHttpUrl } from '../utils/urls.js';

const optionalUrl = z
  .string()
  .trim()
  .max(500)
  .refine((v) => v === '' || isHttpUrl(v), 'Must be a valid http(s) URL')
  .transform((v) => (v === '' ? null : v))
  .nullish();

export const projectSchema = z.object({
  title: z.string().trim().min(1).max(150),
  slug: z
    .string()
    .trim()
    .toLowerCase()
    .max(160)
    .regex(/^[a-z0-9]+(?:-[a-z0-9]+)*$/, 'Use lowercase letters, numbers and hyphens only')
    .optional(),
  summary: z.string().trim().min(1).max(500),
  description: z.string().trim().max(20000).nullish(),
  techStack: z.string().trim().max(300).nullish(),
  imageUrl: optionalUrl,
  githubUrl: optionalUrl,
  liveDemoUrl: optionalUrl,
  caseStudyUrl: optionalUrl,
  isFeatured: z.boolean().default(false),
  isPublished: z.boolean().default(true),
  sortOrder: z.number().int().min(0).max(100000).default(0),
});

export const loginSchema = z.object({
  username: z.string().trim().min(1).max(50),
  password: z.string().min(1).max(200),
});

export const idParamSchema = z.object({
  id: z.coerce.number().int().positive(),
});