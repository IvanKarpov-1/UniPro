<template>
  <!-- Dropdown 1 -->
  <li class="dropdown__item">
    <div class="nav__link dropdown__button fw-bold">
      Create
      <img
        id="blog_arrow"
        class="dropdown__arrow hidden"
        src="../../assets/icons/nav/arrow-sm-right.png"
        alt="arrow"
      />
    </div>
    <div class="dropdown__container">
      <div class="dropdown__content">
        <div class="dropdown__group">
          <a href="/user/create">
            <span class="dropdown__title">User</span>
          </a>
        </div>
        <div class="dropdown__group">
          <a href="/academic/create">
            <span class="dropdown__title">Academic</span>
          </a>
        </div>
        <div class="dropdown__group">
          <a href="/department/create">
            <span class="dropdown__title">Department</span>
          </a>
        </div>
        <div class="dropdown__group">
          <a href="/student-group/create">
            <span class="dropdown__title">Student Group</span>
          </a>
        </div>
        <div class="dropdown__group">
          <a href="/university/create">
            <span class="dropdown__title">University</span>
          </a>
        </div>
      </div>
    </div>
  </li>

  <!-- Dropdown 2 -->
  <li class="dropdown__item">
    <div class="nav__link dropdown__button fw-bold">
      Show&Edit
      <img
        id="products_arrow"
        class="dropdown__arrow hidden"
        src="../../assets/icons/nav/arrow-sm-right.png"
        alt="arrow"
      />
    </div>
    <div class="dropdown__container">
      <div class="dropdown__content">
        <div class="dropdown__group">
          <a href="/users/all">
            <span class="dropdown__title">User</span>
          </a>
        </div>
        <div class="dropdown__group">
          <a href="/academics/all">
            <span class="dropdown__title">Academics</span>
          </a>
        </div>
        <div class="dropdown__group">
          <a href="/departments/all">
            <span class="dropdown__title">Departments</span>
          </a>
        </div>
        <div class="dropdown__group">
          <a href="/student-groups/all">
            <span class="dropdown__title">Student Groups</span>
          </a>
        </div>
        <div class="dropdown__group">
          <a href="/universities/all">
            <span class="dropdown__title">Universities</span>
          </a>
        </div>
      </div>
    </div>
  </li>
</template>