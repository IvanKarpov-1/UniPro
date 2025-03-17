<script>
import { defineComponent } from 'vue';
import Header from '@/components/navbar/Header.vue';
import { getUserInfo } from '@/authService';
import * as Session from 'supertokens-web-js/recipe/session';

export default defineComponent({
  components: {
    Header,
  },
  data() {
    return {
      session: false,
      userId: "",
      payload: null,
      userData: {},
      token: "",
      email: "",
      password: "",
      firstName: "",
      lastName: "",
      patronymic: "",
      role: "student",
      // Выбранные значения и списки для зависимых dropdown-ов:
      selectedUniversityId: null,
      selectedAcademicId: null,
      selectedDepartmentId: null,
      selectedStudentGroupId: null,
      universities: [],
      academics: [],
      departments: [],
      studentGroups: [],
    };
  },
  mounted() {
    getUserInfo().then((data) => {
      this.session = data.session;
      this.userId = data.userId;
      this.userData = data.userData;
    });
    this.fetchUniversities();
  },
  methods: {
    redirectToLogin() {
      window.location.href = "/auth";
    },
    async onLogout() {
      await Session.signOut();
      window.location.reload();
    },
    // Загружаем список университетов
    async fetchUniversities() {
      try {
        const response = await fetch("http://localhost/api/universities");
        if (!response.ok) {
          throw new Error("Failed to fetch universities");
        }
        this.universities = await response.json();
      } catch (error) {
        console.error("Error fetching universities:", error);
      }
    },
    // Загружаем академиков по выбранному университету
    async fetchAcademics(universityId) {
      try {
        const response = await fetch(`http://localhost/api/academics?universityId=${universityId}`);
        if (!response.ok) {
          throw new Error("Failed to fetch academics");
        }
        this.academics = await response.json();
      } catch (error) {
        console.error("Error fetching academics:", error);
      }
    },
    // Загружаем департаменты по выбранному академику
    async fetchDepartments(academicId) {
      try {
        const response = await fetch(`http://localhost/api/departments?academicId=${academicId}`);
        if (!response.ok) {
          throw new Error("Failed to fetch departments");
        }
        this.departments = await response.json();
      } catch (error) {
        console.error("Error fetching departments:", error);
      }
    },
    // Загружаем группы студентов по выбранному департаменту
    async fetchStudentGroups(departmentId) {
      try {
        const response = await fetch(`http://localhost/api/student-groups?departmentId=${departmentId}`);
        if (!response.ok) {
          throw new Error("Failed to fetch student groups");
        }
        this.studentGroups = await response.json();
      } catch (error) {
        console.error("Error fetching student groups:", error);
      }
    },
    // Обработчик отправки формы создания пользователя
    async submitUser() {
      // Валидация обязательных полей
      if (
        !this.email.trim() ||
        !this.password.trim() ||
        !this.firstName.trim() ||
        !this.lastName.trim() ||
        !this.selectedUniversityId
      ) {
        alert("Please fill all required fields.");
        return;
      }
      try {
        const payload = {
          email: this.email,
          password: this.password,
          firstName: this.firstName,
          lastName: this.lastName,
          patronymic: this.patronymic,
          role: this.role,
          universityId: Number(this.selectedUniversityId),
          academicId: this.selectedAcademicId ? Number(this.selectedAcademicId) : null,
          departmentId: this.selectedDepartmentId ? Number(this.selectedDepartmentId) : null,
          studentGroupId: this.selectedStudentGroupId ? Number(this.selectedStudentGroupId) : null,
        };
        console.log(payload);
        const response = await fetch("http://localhost/api/auth/admin/users", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(payload),
        });
        if (!response.ok) {
          throw new Error("Failed to create user");
        }
        await response.json();
        alert("User created successfully!");
        // Сброс формы
        this.email = "";
        this.password = "";
        this.firstName = "";
        this.lastName = "";
        this.patronymic = "";
        this.role = "student";
        this.selectedUniversityId = null;
        this.selectedAcademicId = null;
        this.selectedDepartmentId = null;
        this.selectedStudentGroupId = null;
        this.academics = [];
        this.departments = [];
        this.studentGroups = [];
      } catch (error) {
        console.error("Error creating user:", error);
        alert("Error creating user");
      }
    },
    // При выборе университета подгружаем академиков и сбрасываем зависимые поля
    onUniversityChange() {
      if (this.selectedUniversityId) {
        this.fetchAcademics(this.selectedUniversityId);
      } else {
        this.academics = [];
      }
      this.selectedAcademicId = null;
      this.selectedDepartmentId = null;
      this.selectedStudentGroupId = null;
      this.departments = [];
      this.studentGroups = [];
    },
    // При выборе академика подгружаем департаменты
    onAcademicChange() {
      if (this.selectedAcademicId) {
        this.fetchDepartments(this.selectedAcademicId);
      } else {
        this.departments = [];
      }
      this.selectedDepartmentId = null;
      this.selectedStudentGroupId = null;
      this.studentGroups = [];
    },
    // При выборе департамента подгружаем группы студентов
    onDepartmentChange() {
      if (this.selectedDepartmentId) {
        this.fetchStudentGroups(this.selectedDepartmentId);
      } else {
        this.studentGroups = [];
      }
      this.selectedStudentGroupId = null;
    }
  },
});
</script>

<template>
  <Header />
  <div class="container">
    <main class="freeBird">
      <div class="row">
        <div class="col-md-7 mx-auto">
          <div class="jumbotron p-5">
            <h2 class="h2-responsive">
              <strong>Create User</strong>
            </h2>
            <p>Fill the form below to create a new user</p>
            <div class="card-block">
              <form @submit.prevent="submitUser">
                <!-- Email -->
                <div class="md-form">
                  <input type="email" v-model="email" name="email" id="form-email" class="form-control" required />
                  <label for="form-email">Email</label>
                </div>
                <!-- Password -->
                <div class="md-form">
                  <input type="password" v-model="password" name="password" id="form-password" class="form-control" required />
                  <label for="form-password">Password</label>
                </div>
                <!-- First Name -->
                <div class="md-form">
                  <input type="text" v-model="firstName" name="firstName" id="form-firstname" class="form-control" required />
                  <label for="form-firstname">First Name</label>
                </div>
                <!-- Last Name -->
                <div class="md-form">
                  <input type="text" v-model="lastName" name="lastName" id="form-lastname" class="form-control" required />
                  <label for="form-lastname">Last Name</label>
                </div>
                <!-- Patronymic -->
                <div class="md-form">
                  <input type="text" v-model="patronymic" name="patronymic" id="form-patronymic" class="form-control" />
                  <label for="form-patronymic">Patronymic</label>
                </div>
                <!-- Role -->
                <div class="form-controll">
                  <label for="form-role">Role</label>
                  <select v-model="role" name="role" id="form-role" class="form-select">
                    <option disabled value="">Select a role</option>
                    <option value="student">Student</option>
                    <option value="teacher">Teacher</option>
                    <option value="admin">Admin</option>
                  </select>

                </div>
                <!-- University Dropdown -->
                <div class="form-controll">
                  <label for="form-university">University</label>
                  <select
                    v-model="selectedUniversityId"
                    @change="onUniversityChange"
                    id="form-university"
                    class="form-select"
                    required
                  >
                    <option value="" disabled selected>Select a university</option>
                    <option v-for="uni in universities" :value="uni.universityId" :key="uni.universityId">
                      {{ uni.universityName }}
                    </option>
                  </select>
                </div>
                <!-- Academic Dropdown -->
                <div class="form-controll">
                  <label for="form-academic">Academic</label>
                  <select
                    v-model="selectedAcademicId"
                    @change="onAcademicChange"
                    id="form-academic"
                    class="form-select"
                  >
                    <option value="" disabled selected>Select an academic</option>
                    <option v-for="academic in academics" :value="academic.academicId" :key="academic.academicId">
                      {{ academic.academicName }}
                    </option>
                  </select>
                </div>
                <!-- Department Dropdown -->
                <div class="form-controll">
                  <label for="form-department">Department</label>
                  <select
                    v-model="selectedDepartmentId"
                    @change="onDepartmentChange"
                    id="form-department"
                    class="form-select"
                  >
                    <option value="" disabled selected>Select a department</option>
                    <option v-for="department in departments" :value="department.departmentId" :key="department.departmentId">
                      {{ department.departmentName }}
                    </option>
                  </select>
                </div>
                <!-- Student Group Dropdown -->
                <div class="form-controll">
                  <label for="form-studentGroup">Student Group</label>
                  <select
                    v-model="selectedStudentGroupId"
                    id="form-studentGroup"
                    class="form-select"
                  >
                    <option value="" disabled selected>Select a student group</option>
                    <option v-for="group in studentGroups" :value="group.studentGroupId" :key="group.studentGroupId">
                      {{ group.studentGroupName }}
                    </option>
                  </select>
                </div>
                <!-- Submit Button -->
                <div class="text-left">
                  <button type="submit" class="btn btn-primary">Submit</button>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </main>
  </div>
</template>
