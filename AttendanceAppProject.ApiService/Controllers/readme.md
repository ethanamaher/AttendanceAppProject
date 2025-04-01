# API Overview 

### Attendance Instance
| Method | URI | Operation | Description | Request Body | Response Body |
|--------|-----|-----------|-------------|--------------|----------------|
| GET    | `api/AttendanceInstance` | Read | Get all attendance instances from the database | none | List of `AttendanceInstance` entities (deserialized into `List<AttendanceInstanceDto>` on client) |
| POST   | `api/AttendanceInstance` | Create | Add an attendance instance to the database | `AttendanceInstanceDto` | `AttendanceInstance` entity (deserialized into `AttendanceInstanceDto` if used by client) |

### Class
| Method | URI | Operation | Description | Request Body | Response Body |
|--------|-----|-----------|-------------|--------------|----------------|
| GET    | `api/Class` | Read | Get all classes | none | List of `Class` entities (deserialized into `List<ClassDto>` on client) |
| POST   | `api/Class` | Create | Add a class | `ClassDto` | `Class` entity (deserialized into `ClassDto` if used by client) |

### Professor
| Method | URI | Operation | Description | Request Body | Response Body |
|--------|-----|-----------|-------------|--------------|----------------|
| GET    | `api/Professor` | Read | Get all professors | none | List of `Professor` entities (deserialized into `List<ProfessorDto>` on client) |
| POST   | `api/Professor` | Create | Add a professor | `ProfessorDto` | `Professor` entity (deserialized into `ProfessorDto` if used by client) |

### StudentClass
| Method | URI | Operation | Description | Request Body | Response Body |
|--------|-----|-----------|-------------|--------------|----------------|
| GET    | `api/StudentClass` | Read | Get all student-class mappings | none | List of `StudentClass` entities (deserialized into `List<StudentClassDto>` on client) |
| POST   | `api/StudentClass` | Create | Add a student-class mapping | `StudentClassDto` | `StudentClass` entity (deserialized into `StudentClassDto` if used by client) |
| POST   | `api/StudentClass/check-enrollment` | Read | Check if a student is enrolled in a class | `EnrollmentCheckDto` | `true` or `false` (Boolean) |

### Student
| Method | URI | Operation | Description | Request Body | Response Body |
|--------|-----|-----------|-------------|--------------|----------------|
| GET    | `api/Student` | Read | Get all students | none | List of `Student` entities (deserialized into `List<StudentDto>` on client) |
| POST   | `api/Student` | Create | Add a student | `StudentDto` | `Student` entity (deserialized into `StudentDto` if used by client) |
| POST   | `api/Student/exists` | Read | Check if a student exists in the database | `StudentDto` | `true` or `false` (Boolean) |

### Password
| Method | URI | Operation | Description | Request Body | Response Body |
|--------|-----|-----------|-------------|--------------|----------------|
| GET    | `api/Password` | Read | Get all password records | none | List of `Password` entities (deserialized into `List<PasswordDto>` on client) |
| POST   | `api/Password` | Create | Add a password record | `PasswordDto` | `Password` entity (deserialized into `PasswordDto` if used by client) |
| POST   | `api/Password/validate` | Read | Check if a password is valid for a given class and date | `PasswordDto` (containing `ClassId`, `PasswordText`, and `DateAssigned` sent over by client side) | `true` or `false` (Boolean) |

Note: The full response body of standard POST requests which simply add another resource to the database is the HTTP 201 created code and a Location header pointing to where the new resource can be found, and the new resource itself in the response body.

Note: UUIDs MUST be created on the server side only. The client side can retrieve UUIDs upon retrieval of a resource from the database in DTO form but it should not create any. When the client side sends a DTO to the API in order to add a new resource, it should not contain any UUID as that will be auto-generated from the server side only.

## Example Usage in Blazor Front-End
To retrieve a list of all students:
```
var studentList = await Http.GetFromJsonAsync<List<StudentDto>>("api/Student");
```

To add a student to the database:
```
await Http.PostAsJsonAsync("api/Student", newStudentDto);
```

To see if a student exists in the database:
```
var existsResponse = await Http.PostAsJsonAsync("api/Student/exists", inputStudentDto);
```

To check if a student is enrolled in a particular class:
```
var isEnrolled = await Http.PostAsJsonAsync("api/StudentClass/check-enrollment", new EnrollmentCheckDto { Student = newStudentDto, Class = databaseClassDto });
``` 
or 
```
var checkDto = new EnrollmentCheckDto
{
    Student = newStudentDto,
    Class = databaseClassDto
};
await Http.PostAsJsonAsync("api/studentclass/check-enrollment", checkDto); 
```

## Postman Examples
<img width="912" alt="image" src="https://github.com/user-attachments/assets/21523f7c-93e9-4309-9580-f441709a8354" />

<img width="914" alt="image" src="https://github.com/user-attachments/assets/38e3d657-63dd-4c03-8e5a-1640b81c85d1" />

<img width="906" alt="image" src="https://github.com/user-attachments/assets/239b5826-2554-487b-aad7-c3ac6958ae05" />



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
