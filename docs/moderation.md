# Moderation Module

## Overview

The Moderation module provides a centralized moderation system for the VahanX platform. It handles reports, moderation cases, actions, and user restrictions.

## Report Lifecycle

Reports follow this status flow:

```
Pending → UnderReview → Resolved
                    → Rejected
                    → Dismissed
                    → Escalated
```

### Report Statuses

| Status | Description |
|--------|-------------|
| Pending | Newly created report |
| UnderReview | Assigned and being reviewed |
| Resolved | Report has been resolved |
| Rejected | Report was rejected |
| Dismissed | Report was dismissed |
| Escalated | Report has been escalated |

### Report Priorities

| Priority | Description |
|----------|-------------|
| Low | Low priority |
| Normal | Normal priority |
| High | High priority |
| Critical | Critical priority |

## Moderation Case Lifecycle

```
Open → InProgress → Resolved
                → Escalated
                → Closed
```

### Resolution Types

- NoAction
- WarningIssued
- ContentEdited
- ContentHidden
- ContentRejected
- ContentRemoved
- UserRestricted
- UserSuspended
- ListingSuspended
- AdvertisementRejected
- Escalated

## Moderation Actions

Every important moderation action is recorded with:
- Actor (who performed the action)
- Target (what was affected)
- Action type
- Reason
- Timestamp

### Action Types

- Warn
- Hide
- Unhide
- Reject
- Remove
- Suspend
- Unsuspend
- Restrict
- Unrestrict
- Approve
- Escalate

## User Restrictions

User restrictions provide controlled suspension mechanisms:

| Restriction Type | Description |
|------------------|-------------|
| Warning | Warning issued |
| ListingRestriction | Cannot create listings |
| MessagingRestriction | Cannot send messages |
| AdvertisementRestriction | Cannot create advertisements |
| SellVehicleRestriction | Cannot use sell vehicle feature |
| FullPlatformRestriction | Full platform suspension |

## Listing Moderation

Moderators can:
- Submit for review
- Approve
- Reject
- Hide
- Restore
- Suspend
- Archive

## Review Moderation

Moderators can:
- Report review
- Hide review
- Restore review
- Reject review

## Advertisement Moderation

Moderators can:
- Review
- Approve
- Reject
- Pause
- Resume
- Cancel

## Moderation History

All moderation actions are recorded in an append-only history:
- Moderation case ID
- Old status
- New status
- Action
- Changed by
- Reason
- Timestamp

## APIs

### Reports
- GET /api/v1/admin/moderation/reports
- GET /api/v1/admin/moderation/reports/{id}
- POST /api/v1/admin/moderation/reports
- POST /api/v1/admin/moderation/reports/{id}/assign
- POST /api/v1/admin/moderation/reports/{id}/resolve
- POST /api/v1/admin/moderation/reports/{id}/reject
- POST /api/v1/admin/moderation/reports/{id}/escalate

### Cases
- GET /api/v1/admin/moderation/cases
- GET /api/v1/admin/moderation/cases/{id}
- POST /api/v1/admin/moderation/cases
- POST /api/v1/admin/moderation/cases/{id}/assign
- POST /api/v1/admin/moderation/cases/{id}/action
- POST /api/v1/admin/moderation/cases/{id}/resolve

### Listing Moderation
- GET /api/v1/admin/moderation/listings/pending
- POST /api/v1/admin/moderation/listings/{id}/approve
- POST /api/v1/admin/moderation/listings/{id}/reject
- POST /api/v1/admin/moderation/listings/{id}/hide
- POST /api/v1/admin/moderation/listings/{id}/restore

### User Restrictions
- POST /api/v1/admin/users/{id}/restrict
- GET /api/v1/admin/users/{id}/restrictions
- POST /api/v1/admin/users/restrictions/{id}/deactivate
