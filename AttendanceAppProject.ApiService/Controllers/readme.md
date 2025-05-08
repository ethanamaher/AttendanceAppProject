# API Overview 

### AttendanceInstance API

| Method | URI                                                                 | Operation | Description                                                                | Request Body              | Response Body                                                                 |
|--------|---------------------------------------------------------------------|-----------|----------------------------------------------------------------------------|---------------------------|--------------------------------------------------------------------------------|
| GET    | /api/AttendanceInstance                                             | Read      | Get all attendance records                                                 | None                      | IEnumerable\<AttendanceInstance\> (can be deserialized into List\<AttendanceInstanceDto\> on client side) |
| GET    | /api/AttendanceInstance/class/{classId}                             | Read      | Get attendance records by class ID                                         | None                      | IEnumerable\<AttendanceInstance\> (can be deserialized into List\<AttendanceInstanceDto\> on client) |
| GET    | /api/AttendanceInstance/student/{studentId}?date=...&classId=...   | Read      | Get a student's attendance, optionally filtered by date and class ID      | None                      | IEnumerable\<AttendanceInstance\> (can be deserialized into List\<AttendanceInstanceDto\>) |
| GET    | /api/AttendanceInstance/class/{classId}/absent-on-date/{dateStr}   | Read      | Get students absent on a specific date for a class                         | None                      | IEnumerable\<Student\> (can be deserialized into List\<StudentDto\>)                     |
| GET    | /api/AttendanceInstance/excused-absences?classId=...&date=...      | Read      | Get excused absences (optional filters)                                    | None                      | IEnumerable\<AttendanceInstance\> (can be deserialized into List\<AttendanceInstanceDto\>) |
| GET    | /api/AttendanceInstance/lates?classId=...&date=...                 | Read      | Get late attendance records (optional filters)                             | None                      | IEnumerable\<AttendanceInstance\> (can be deserialized into List\<AttendanceInstanceDto\>) |
| GET    | /api/AttendanceInstance/class/{classId}/consecutive-absences/{n}   | Read      | Get students with n consecutive absences                                   | None                      | IEnumerable\<Student\> (can be deserialized into List\<StudentDto\>)                     |
| GET    | /api/AttendanceInstance/class/{classId}/total-absences/{n}         | Read      | Get students with n total absences                                         | None                      | IEnumerable\<Student\> (can be deserialized into List\<StudentDto\>)                     |
| POST   | /api/AttendanceInstance                                             | Create    | Add a new attendance record                                                | AttendanceInstanceDto     | AttendanceInstance (can be deserialized into AttendanceInstanceDto)              |
| POST   | /api/AttendanceInstance/absent-or-late                              | Create    | Add a record for an absent/late student (entered by professor)            | AttendanceInstanceDto     | AttendanceInstance (can be deserialized into AttendanceInstanceDto)              |
| PUT    | /api/AttendanceInstance/{id}                                       | Update    | Update an existing attendance record                                       | AttendanceInstanceDto     | AttendanceInstance (can be deserialized into AttendanceInstanceDto)              |
| DELETE | /api/AttendanceInstance/{id}                                       | Delete    | Delete a specific attendance record                                        | None                      | NoContent / NotFound                                                             |

### Class API

| Method | URI                              | Operation | Description                            | Request Body | Response Body |
|--------|----------------------------------|-----------|----------------------------------------|--------------|----------------|
| GET    | /api/Class                       | Read      | Get all classes                        | None         | `IEnumerable<Class>` |
| GET    | /api/Class/{id}                  | Read      | Get a class by ID                      | None         | Class (can be deserialized into `ClassDto`) |
| GET    | /api/Class/professor/{profId}    | Read      | Get all classes for a professor        | None         | `IEnumerable<Class>` |
| POST   | /api/Class                       | Create    | Add a new class                        | ClassDto     | Class |
| POST   | /api/Class/exists                | Read      | Check if a class exists                | Guid         | Boolean |
| PUT    | /api/Class/{id}                  | Update    | Update class by ID                     | ClassDto     | Class |
| DELETE | /api/Class/{id}                  | Delete    | Delete class by ID                     | None         | NoContent |

---

### Professor API

| Method | URI                           | Operation | Description               | Request Body   | Response Body |
|--------|-------------------------------|-----------|---------------------------|----------------|----------------|
| GET    | /api/Professor                | Read      | Get all professors        | None           | `IEnumerable<Professor>` |
| GET    | /api/Professor/{UtdId}        | Read      | Get professor by UTD ID   | None           | Professor|
| POST   | /api/Professor                | Create    | Add new professor         | ProfessorDto   | Professor |
| POST   | /api/Professor/login          | Read      | Login with credentials    | ProfessorDto   | Professor |
| PUT    | /api/Professor/{UtdId}        | Update    | Update professor          | ProfessorDto   | Professor |
| DELETE | /api/Professor/{UtdId}        | Delete    | Delete professor          | None           | NoContent |

---

### Password API

| Method | URI                               | Operation | Description                            | Request Body   | Response Body |
|--------|-----------------------------------|-----------|----------------------------------------|----------------|----------------|
| GET    | /api/Password                     | Read      | Get all password entries               | None           | `IEnumerable<Password>` |
| GET    | /api/Password/class/{classId}     | Read      | Get password by class ID               | None           | Password|
| POST   | /api/Password                     | Create    | Add new password                       | PasswordDto    | Password|
| POST   | /api/Password/validate            | Read      | Validate password                      | PasswordDto    | Boolean |
| PUT    | /api/Password/{classId}           | Update    | Update password by class ID            | PasswordDto    | Password |
| DELETE | /api/Password/{id}                | Delete    | Delete password by ID                  | None           | NoContent |

---

### Student API

| Method | URI                        | Operation | Description                  | Request Body | Response Body |
|--------|----------------------------|-----------|------------------------------|--------------|----------------|
| GET    | /api/Student               | Read      | Get all students             | None         | `IEnumerable<Student>`|
| POST   | /api/Student               | Create    | Add a new student            | StudentDto   | Student |
| POST   | /api/Student/exists        | Read      | Check if student exists      | string       | Boolean |
| PUT    | /api/Student/{UtdId}       | Update    | Update student by ID         | StudentDto   | Student |
| DELETE | /api/Student/{UtdId}       | Delete    | Delete student by ID         | None         | NoContent |

---

### StudentClass API

| Method | URI                                      | Operation | Description                        | Request Body         | Response Body |
|--------|------------------------------------------|-----------|------------------------------------|----------------------|----------------|
| GET    | /api/StudentClass                        | Read      | Get all student-class mappings     | None                 | IEnumerable<StudentClass> (`List<StudentClassDto>`) |
| POST   | /api/StudentClass                        | Create    | Add a student-class mapping        | StudentClassDto      | StudentClass |
| POST   | /api/StudentClass/check-enrollment       | Read      | Check if student is enrolled       | EnrollmentCheckDto   | Boolean |
| PUT    | /api/StudentClass/{studentClassId}       | Update    | Update a student-class mapping     | StudentClassDto      | StudentClass |
| DELETE | /api/StudentClass/{studentClassId}       | Delete    | Delete a student-class mapping     | None                 | NoContent |

### QuizInstance

| Method | URI                                | Operation | Description                                                      | Request Body        | Response Body                          |
|--------|-------------------------------------|-----------|------------------------------------------------------------------|---------------------|----------------------------------------|
| GET    | `api/QuizInstance`                 | Read      | Get all quiz instances                                           | none                | `IEnumerable<QuizInstance>` (can deserialize into `List<QuizInstanceDto>`) |
| GET    | `api/QuizInstance/{ClassId}`       | Read      | Get quiz instance for a class by `ClassId`                       | none                | `QuizInstance` (can deserialize into `QuizInstanceDto`) |
| POST   | `api/QuizInstance`                 | Create    | Add a new quiz instance                                          | `QuizInstanceDto`   | `QuizInstance`                         |
| PUT    | `api/QuizInstance/{QuizId}`        | Update    | Update class association only of an existing quiz instance       | `QuizInstanceDto` (only `ClassId` used) | `QuizInstance`                         |
| PUT    | `api/QuizInstance/{quizId}`        | Update    | Update full quiz instance (entire object)                        | `QuizInstanceDto`   | `QuizInstance`                         |
| DELETE | `api/QuizInstance/{QuizId}`        | Delete    | Delete quiz instance by `QuizId`                                 | none                | `204 NoContent` or `404 NotFound`      |

### QuizQuestion

| Method | URI                                       | Operation | Description                                               | Request Body         | Response Body                        |
|--------|--------------------------------------------|-----------|-----------------------------------------------------------|----------------------|-------------------------------------|
| GET    | `api/QuizQuestion/{QuizId}`                | Read      | Get all quiz questions by `QuizId`                        | none                 | `IEnumerable<QuizQuestion>` (can be deserialized into `List<QuizQuestionDto>`) |
| POST   | `api/QuizQuestion`                         | Create    | Add a new quiz question                                   | `QuizQuestionDto`    | `QuizQuestion`                      |
| PUT    | `api/QuizQuestion/{questionId}`            | Update    | Update a quiz question by `questionId`                    | `QuizQuestionDto`    | `QuizQuestion`                      |
| DELETE | `api/QuizQuestion/{QuestionId}`            | Delete    | Delete quiz question by `QuestionId`                      | none                 | `204 NoContent` or `404 NotFound`   |

### QuizAnswer

| Method | URI                                  | Operation | Description                                               | Request Body       | Response Body                        |
|--------|---------------------------------------|-----------|-----------------------------------------------------------|--------------------|-------------------------------------|
| GET    | `api/QuizAnswer/{QuestionId}`         | Read      | Get all quiz answers by `QuestionId`                      | none               | `IEnumerable<QuizAnswer>` (can be deserialized into `List<QuizAnswerDto>`) |
| POST   | `api/QuizAnswer`                      | Create    | Add a new quiz answer                                     | `QuizAnswerDto`    | `QuizAnswer`                        |
| PUT    | `api/QuizAnswer/{answerId}`           | Update    | Update quiz answer by `answerId`                          | `QuizAnswerDto`    | `QuizAnswer`                        |
| DELETE | `api/QuizAnswer/{AnswerId}`           | Delete    | Delete quiz answer by `AnswerId`                          | none               | `204 NoContent` or `404 NotFound`   |

Note: The full response body of standard create POST requests which simply add another resource to the database is the HTTP 201 created code and a Location header pointing to where the new resource can be found, and the new resource itself in the response body. From the Blazor front-end we can check if a POST request was successful this way:
```
var response = await Http.PostAsJsonAsync("api/student", newStudentDto);

if (response.StatusCode == System.Net.HttpStatusCode.Created)
{
    // POST was successful
    Console.WriteLine("A new student was added successfully!");
}
else
{
    // Handle failure
    Console.WriteLine($"Failed to add student. Status: {response.StatusCode}");
}
```

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
