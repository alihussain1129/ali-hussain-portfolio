import { getPool, sql } from '../config/db.js';

const COLUMNS = `
  Id AS id, Title AS title, Slug AS slug, Summary AS summary,
  Description AS description, TechStack AS techStack, ImageUrl AS imageUrl,
  GithubUrl AS githubUrl, LiveDemoUrl AS liveDemoUrl, CaseStudyUrl AS caseStudyUrl,
  IsFeatured AS isFeatured, IsPublished AS isPublished, SortOrder AS sortOrder,
  CreatedAt AS createdAt, UpdatedAt AS updatedAt`;

function bindProject(request, p) {
  return request
    .input('title', sql.NVarChar(150), p.title)
    .input('slug', sql.NVarChar(160), p.slug)
    .input('summary', sql.NVarChar(500), p.summary)
    .input('description', sql.NVarChar(sql.MAX), p.description ?? null)
    .input('techStack', sql.NVarChar(300), p.techStack ?? null)
    .input('imageUrl', sql.NVarChar(500), p.imageUrl ?? null)
    .input('githubUrl', sql.NVarChar(500), p.githubUrl ?? null)
    .input('liveDemoUrl', sql.NVarChar(500), p.liveDemoUrl ?? null)
    .input('caseStudyUrl', sql.NVarChar(500), p.caseStudyUrl ?? null)
    .input('isFeatured', sql.Bit, p.isFeatured)
    .input('isPublished', sql.Bit, p.isPublished)
    .input('sortOrder', sql.Int, p.sortOrder);
}

export async function listProjects({ includeUnpublished = false } = {}) {
  const pool = await getPool();
  const result = await pool
    .request()
    .input('all', sql.Bit, includeUnpublished)
    .query(`SELECT ${COLUMNS} FROM dbo.Projects
            WHERE (@all = 1 OR IsPublished = 1)
            ORDER BY IsFeatured DESC, SortOrder ASC, CreatedAt DESC`);
  return result.recordset;
}

export async function findBySlug(slug) {
  const pool = await getPool();
  const result = await pool
    .request()
    .input('slug', sql.NVarChar(160), slug)
    .query(`SELECT ${COLUMNS} FROM dbo.Projects WHERE Slug = @slug`);
  return result.recordset[0] ?? null;
}

export async function findById(id) {
  const pool = await getPool();
  const result = await pool
    .request()
    .input('id', sql.Int, id)
    .query(`SELECT ${COLUMNS} FROM dbo.Projects WHERE Id = @id`);
  return result.recordset[0] ?? null;
}

export async function insertProject(p) {
  const pool = await getPool();
  const result = await bindProject(pool.request(), p).query(`
    INSERT INTO dbo.Projects
      (Title, Slug, Summary, Description, TechStack, ImageUrl, GithubUrl,
       LiveDemoUrl, CaseStudyUrl, IsFeatured, IsPublished, SortOrder)
    OUTPUT INSERTED.Id AS id
    VALUES
      (@title, @slug, @summary, @description, @techStack, @imageUrl, @githubUrl,
       @liveDemoUrl, @caseStudyUrl, @isFeatured, @isPublished, @sortOrder)`);
  return result.recordset[0].id;
}

export async function updateProject(id, p) {
  const pool = await getPool();
  const request = bindProject(pool.request(), p).input('id', sql.Int, id);
  const result = await request.query(`
    UPDATE dbo.Projects SET
      Title = @title, Slug = @slug, Summary = @summary, Description = @description,
      TechStack = @techStack, ImageUrl = @imageUrl, GithubUrl = @githubUrl,
      LiveDemoUrl = @liveDemoUrl, CaseStudyUrl = @caseStudyUrl,
      IsFeatured = @isFeatured, IsPublished = @isPublished, SortOrder = @sortOrder,
      UpdatedAt = SYSUTCDATETIME()
    WHERE Id = @id`);
  return result.rowsAffected[0] > 0;
}

export async function deleteProject(id) {
  const pool = await getPool();
  const result = await pool
    .request()
    .input('id', sql.Int, id)
    .query('DELETE FROM dbo.Projects WHERE Id = @id');
  return result.rowsAffected[0] > 0;
}