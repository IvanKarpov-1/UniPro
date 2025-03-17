<template>
    <!-- Dropdown 1 -->
    <li class="dropdown__item">
      <div class="nav__link dropdown__button fw-bold">
        Tasks
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
            <a href="/teacher-task/create">
              <span class="dropdown__title">Create Task</span>
            </a>
          </div>
          <div class="dropdown__group">
            <a href="/teacher-task/all">
              <span class="dropdown__title">All Tasks</span>
            </a>
          </div>
        </div>
      </div>
    </li>
  
    <!-- Dropdown 2 -->
    <li class="dropdown__item">
      <div class="nav__link dropdown__button fw-bold">
        Timetables
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
            <a href="/student-timetable">
              <span class="dropdown__title">Timetables</span>
            </a>
          </div>
        </div>
      </div>
    </li>
    <li>
        <a href="/report/create" class="nav__link fw-bold" aria-disabled="true">Report</a>
    </li>
  </template>
  