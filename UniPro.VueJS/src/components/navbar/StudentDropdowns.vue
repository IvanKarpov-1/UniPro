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
          <a href="/student-tasks/done">
            <span class="dropdown__title">Done</span>
          </a>
        </div>
        <div class="dropdown__group">
          <a href="/student-tasks/inprogress">
            <span class="dropdown__title">To do</span>
          </a>
        </div>
        <div class="dropdown__group">
          <a href="/student-tasks/missed">
            <span class="dropdown__title">Missed</span>
          </a>
        </div>
      </div>
    </div>
  </li>

  <!-- Dropdown 2 -->
  <li class="dropdown__item">
    <div class="nav__link dropdown__button fw-bold">
      Timetable & Group
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
            <span class="dropdown__title">Current timetable</span>
          </a>
        </div>
        <div class="dropdown__group">
          <a href="/student-group">
            <span class="dropdown__title">Group List</span>
          </a>
        </div>
      </div>
    </div>
  </li>
</template>
