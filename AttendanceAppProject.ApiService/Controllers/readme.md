# API Overview 

### Attendance Instance
| Method | URI | Operation | Description | Request Body | Response Body |
|--------|-----|-----------|-------------|--------------|----------------|
| GET    | `api/AttendanceInstance` | Read | Get all attendance instances from the database | none | List of `AttendanceInstance` entities (deserialized into `List<AttendanceInstanceDto>` on client) |
| POST   | `api/AttendanceInstance` | Create | Add an attendance instance to the database | `AttendanceInstanceDto` | `AttendanceInstance` entity (deserialized into `AttendanceInstanceDto` on client) |

### Class
| Method | URI | Operation | Description | Request Body | Response Body |
|--------|-----|-----------|-------------|--------------|----------------|
| GET    | `api/Class` | Read | Get all classes | none | List of `Class` entities (deserialized into `List<ClassDto>` on client) |
| POST   | `api/Class` | Create | Add a class | `ClassDto` | `Class` entity (deserialized into `ClassDto` on client) |

### Professor
| Method | URI | Operation | Description | Request Body | Response Body |
|--------|-----|-----------|-------------|--------------|----------------|
| GET    | `api/Professor` | Read | Get all professors | none | List of `Professor` entities (deserialized into `List<ProfessorDto>` on client) |
| POST   | `api/Professor` | Create | Add a professor | `ProfessorDto` | `Professor` entity (deserialized into `ProfessorDto` on client) |

### StudentClass
| Method | URI | Operation | Description | Request Body | Response Body |
|--------|-----|-----------|-------------|--------------|----------------|
| GET    | `api/StudentClass` | Read | Get all student-class mappings | none | List of `StudentClass` entities (deserialized into `List<StudentClassDto>` on client) |
| POST   | `api/StudentClass` | Create | Add a student-class mapping | `StudentClassDto` | `StudentClass` entity (deserialized into `StudentClassDto` on client) |
| POST   | `api/StudentClass/check-enrollment` | Read | Check if a student is enrolled in a class | `EnrollmentCheckDto` | `true` or `false` (Boolean) |

### Student
| Method | URI | Operation | Description | Request Body | Response Body |
|--------|-----|-----------|-------------|--------------|----------------|
| GET    | `api/Student` | Read | Get all students | none | List of `Student` entities (deserialized into `List<StudentDto>` on client) |
| POST   | `api/Student` | Create | Add a student | `StudentDto` | `Student` entity (deserialized into `StudentDto` on client) |
| POST   | `api/Student/exists` | Read | Check if a student exists in the database | `StudentDto` | `true` or `false` (Boolean) |

## API Flow
```
Blazor Frontend 
    ↓ 
HTTP Request (with DTO serialized to JSON, if sending data with the request) 
    ↓ 
ASP.NET Core Web API 
    ↓ 
Processes Request → (returns entity or DTO, if sending data with the response) → Serializes to JSON 
    ↓ 
HTTP Response (JSON payload) 
    ↓ 
Blazor Frontend (deserializes JSON into DTO if receiving data)
```
