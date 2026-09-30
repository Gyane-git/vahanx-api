# CMS Module

## Overview

The CMS module controls public VahanX content. It manages pages, articles, FAQs, banners, promotions, and announcements.

## Content Types

### Pages
Static pages with SEO support.

### Articles
Blog posts and news articles with categories.

### FAQs
Frequently asked questions with categories.

### Banners
Promotional banners with scheduling.

### Promotions
VahanX-managed promotional content (distinct from paid advertisements).

### Announcements
Platform-wide messages with priority levels.

## Content Lifecycle

```
Draft → PendingReview → Published → Unpublished → Archived
```

### Statuses

| Status | Description |
|--------|-------------|
| Draft | Content is being created |
| PendingReview | Content is awaiting review |
| Published | Content is live |
| Unpublished | Content is hidden |
| Archived | Content is archived |

## Publishing Workflow

Authorized content managers can:
- Create content
- Edit content
- Submit for review
- Publish content
- Unpublish content
- Archive content

## SEO

CMS pages and articles support:
- Slug (unique, URL-safe)
- SEO title
- SEO description

## Media

CMS media uses the existing Media infrastructure:
- Banner images
- Article images
- Page images
- Promotion media

No binary files are stored in SQL Server.

## Content Security

Content fields may contain HTML/rich text. The system implements:
- Content validation
- HTML sanitization
- XSS prevention
- External link validation
- Unsafe embedded content restriction

## Public APIs

- GET /api/v1/content/pages/{slug}
- GET /api/v1/content/articles
- GET /api/v1/content/articles/{slug}
- GET /api/v1/content/categories
- GET /api/v1/content/faqs
- GET /api/v1/content/banners
- GET /api/v1/content/promotions
- GET /api/v1/content/announcements

## Admin APIs

### Pages
- POST /api/v1/admin/cms/pages
- GET /api/v1/admin/cms/pages
- GET /api/v1/admin/cms/pages/{id}
- PUT /api/v1/admin/cms/pages/{id}
- POST /api/v1/admin/cms/pages/{id}/submit
- POST /api/v1/admin/cms/pages/{id}/publish
- POST /api/v1/admin/cms/pages/{id}/unpublish
- POST /api/v1/admin/cms/pages/{id}/archive

### Articles
- POST /api/v1/admin/cms/articles
- GET /api/v1/admin/cms/articles
- GET /api/v1/admin/cms/articles/{id}
- PUT /api/v1/admin/cms/articles/{id}
- POST /api/v1/admin/cms/articles/{id}/submit
- POST /api/v1/admin/cms/articles/{id}/publish
- POST /api/v1/admin/cms/articles/{id}/unpublish
- POST /api/v1/admin/cms/articles/{id}/archive

### FAQs
- POST /api/v1/admin/cms/faqs
- PUT /api/v1/admin/cms/faqs/{id}
- DELETE /api/v1/admin/cms/faqs/{id}

### Banners
- POST /api/v1/admin/cms/banners
- PUT /api/v1/admin/cms/banners/{id}
- POST /api/v1/admin/cms/banners/{id}/activate
- POST /api/v1/admin/cms/banners/{id}/deactivate

### Promotions
- POST /api/v1/admin/cms/promotions
- PUT /api/v1/admin/cms/promotions/{id}

### Announcements
- POST /api/v1/admin/cms/announcements
- PUT /api/v1/admin/cms/announcements/{id}
- POST /api/v1/admin/cms/announcements/{id}/publish
- POST /api/v1/admin/cms/announcements/{id}/unpublish

## Scheduled Content

Banners, promotions, and announcements support StartAt/EndAt dates. Public APIs always check current time even if a background job has not executed.

## Announcement Priorities

- Low
- Normal
- High
- Critical
