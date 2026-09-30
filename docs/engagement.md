# VahanX Engagement Module

## Architecture

The Engagement module follows the same Clean Architecture principles as the rest of the VahanX platform.

## Enquiry

### Overview
An enquiry represents a user expressing interest in a vehicle listing.

### Status Lifecycle
```
New → Open → InProgress → Responded → Closed
                                      → Cancelled
```

### Enquiry Types
- General
- Price
- Availability
- VehicleCondition
- Financing
- TestDrive
- Other

## Conversation & Messaging

### Overview
Conversations link users with sellers/dealers for communication.

### Conversation Types
- Enquiry
- Listing
- TestDrive
- Support

### Message Types
- Text
- System
- Attachment

### Message Rules
- Only participants can access a conversation
- Only the sender can edit/delete their own messages
- Messages are paginated
- Deleted messages show "[deleted]" placeholder

## Test Drive

### Overview
Test drive requests for vehicle listings.

### Status Lifecycle
```
Requested → Pending → Confirmed → Completed
                        → Rescheduled
                        → Cancelled
                        → Rejected
                        → NoShow
```

### Test Drive Slots
- Slots define available time windows
- Capacity controls concurrent bookings
- Confirmed slots prevent double booking

## Notification

### Overview
Reusable notification system for marketplace events.

### Notification Types
- NewEnquiry
- EnquiryResponse
- NewMessage
- ListingPublished
- ListingRejected
- ListingPriceChanged
- WishlistPriceChanged
- TestDriveRequested
- TestDriveConfirmed
- TestDriveRescheduled
- TestDriveCancelled
- VerificationCompleted
- InspectionCompleted
- ReviewPublished
- System

### Notification Channels
- Push
- Email
- SMS
- InApp

### Push Token Platforms
- Android
- iOS
- Web

## Authorization Rules

### Users
- Access own enquiries
- Access own conversations
- Access own test drives
- Access own notifications
- Manage own notification preferences

### Sellers
- Access enquiries for their listings
- Access conversations they participate in
- Manage relevant test drive requests

### Dealer Staff
- Access authorized dealer enquiries
- Access authorized dealer test drives
- Access authorized dealer conversations

## Privacy Rules

### Never Expose
- Other users' phone/email unnecessarily
- Private conversations
- Internal notification metadata
- Push tokens
- Private dealer information
- Authentication details

## API Endpoints

### Enquiries
- `GET /api/v1/enquiries` - Get all enquiries
- `GET /api/v1/enquiries/{id}` - Get enquiry by ID
- `POST /api/v1/enquiries` - Create enquiry
- `PUT /api/v1/enquiries/{id}` - Update enquiry
- `DELETE /api/v1/enquiries/{id}` - Delete enquiry
- `POST /api/v1/enquiries/{id}/respond` - Respond to enquiry
- `POST /api/v1/enquiries/{id}/close` - Close enquiry
- `POST /api/v1/enquiries/{id}/cancel` - Cancel enquiry
- `GET /api/v1/enquiries/listing/{listingId}` - Get listing enquiries
- `GET /api/v1/enquiries/seller/{sellerId}` - Get seller enquiries
- `GET /api/v1/enquiries/dealer/{dealerId}` - Get dealer enquiries

### Conversations
- `GET /api/v1/conversations` - Get user conversations
- `GET /api/v1/conversations/{id}` - Get conversation by ID
- `POST /api/v1/conversations` - Create conversation
- `GET /api/v1/conversations/{id}/messages` - Get messages
- `POST /api/v1/conversations/{id}/messages` - Send message
- `PUT /api/v1/messages/{id}` - Update message
- `DELETE /api/v1/messages/{id}` - Delete message

### Test Drives
- `GET /api/v1/test-drives` - Get all test drives
- `GET /api/v1/test-drives/{id}` - Get test drive by ID
- `POST /api/v1/test-drives` - Create test drive request
- `PUT /api/v1/test-drives/{id}` - Update test drive
- `DELETE /api/v1/test-drives/{id}` - Delete test drive
- `POST /api/v1/test-drives/{id}/confirm` - Confirm test drive
- `POST /api/v1/test-drives/{id}/reschedule` - Reschedule test drive
- `POST /api/v1/test-drives/{id}/cancel` - Cancel test drive
- `POST /api/v1/test-drives/{id}/complete` - Complete test drive
- `GET /api/v1/test-drives/listing/{listingId}` - Get listing test drives
- `GET /api/v1/test-drives/dealer/{dealerId}` - Get dealer test drives

### Notifications
- `GET /api/v1/notifications` - Get user notifications
- `GET /api/v1/notifications/unread-count` - Get unread count
- `POST /api/v1/notifications/{id}/read` - Mark as read
- `POST /api/v1/notifications/read-all` - Mark all as read
- `GET /api/v1/notification-preferences` - Get preferences
- `PUT /api/v1/notification-preferences` - Update preferences
- `POST /api/v1/push-tokens` - Register push token
- `DELETE /api/v1/push-tokens/{id}` - Delete push token

## Database Relationships

```
VehicleListing (1) ← (many) Enquiry
VehicleListing (1) ← (many) TestDrive
Enquiry (1) ← (1) Conversation
Conversation (1) ← (many) ConversationParticipant
Conversation (1) ← (many) Message
Message (1) ← (many) MessageAttachment
Dealer (1) ← (many) TestDriveSlot
User (1) ← (many) Notification
User (1) ← (many) NotificationPreference
User (1) ← (many) PushToken
```

## Future Extension Points

- SignalR for real-time messaging
- Firebase/APNs push provider integration
- Email/SMS notification providers
- Advanced conversation features
