<template>
  <q-page class="full-height full-width flex flex-center">
    <div class="container full-height full-width">
      <q-dialog v-model="showDelete" :persistent="deleteProcess">
        <q-card>
          <q-card-section class="row items-center">
            <span class="text-h6">Delete {{ getNameDelete }}?</span>
          </q-card-section>

          <q-separator />

          <q-card-section>
            <span
              >This can’t be undone and it will be removed from database.</span
            >
          </q-card-section>

          <q-card-actions align="right">
            <q-btn
              flat
              :disable="deleteProcess"
              label="Cancel"
              color="primary"
              v-close-popup
            />
            <q-btn
              flat
              label="Delete"
              color="negative"
              @click="deleteEmployee"
              :loading="deleteProcess"
            />
          </q-card-actions>
        </q-card>
      </q-dialog>

      <q-dialog v-model="showEdit" persistent full-width full-height>
        <q-layout view="hHh lpR fFf" container class="bg-white">
          <q-header class="bg-accent">
            <q-toolbar>
              <q-toolbar-title></q-toolbar-title>
              <q-btn
                class="q-mr-lg"
                flat
                round
                dense
                icon="visibility"
                @click="openViewDialog(editObj.id)"
              />
              <q-btn flat round dense icon="close" @click="closeEditDialog" />
            </q-toolbar>
          </q-header>

          <q-page-container>
            <q-page>
              <edit-employee
                v-if="showEdit"
                @updateSuccess="updateReceive"
                :statusUpdateTransfer="statusUpdate"
                :employeeTransfer="editObj"
              />
            </q-page>
          </q-page-container>

          <q-footer class="bg-accent text-white">
            <q-toolbar class="flex flex-center">
              <q-btn
                :loading="statusUpdate"
                dense
                color="primary"
                label="Save"
                class="q-px-lg"
                @click="saveUpdate"
              />
            </q-toolbar>
          </q-footer>
        </q-layout>
      </q-dialog>

      <q-dialog v-model="showInsert" persistent full-width full-height>
        <q-layout view="hHh lpR fFf" container class="bg-white">
          <q-header class="bg-accent">
            <q-toolbar>
              <q-toolbar-title></q-toolbar-title>
              <q-btn flat round dense icon="close" @click="closeInsertDialog" />
            </q-toolbar>
          </q-header>

          <q-page-container>
            <q-page>
              <new-employee
                v-if="showInsert"
                @insertSuccess="insertReceive"
                :statusInsertTransfer="statusInsert"
              />
            </q-page>
          </q-page-container>

          <q-footer class="bg-accent text-white">
            <q-toolbar class="flex flex-center">
              <q-btn
                :loading="statusInsert"
                dense
                color="primary"
                label="Save"
                class="q-px-lg"
                @click="saveInsert"
              />
            </q-toolbar>
          </q-footer>
        </q-layout>
      </q-dialog>

      <q-dialog v-model="showView" full-height>
        <q-layout view="hHh lpR fFf" container class="bg-white">
          <q-header class="bg-accent">
            <q-toolbar>
              <q-toolbar-title></q-toolbar-title>
              <q-btn v-close-popup flat round dense icon="close" />
            </q-toolbar>
          </q-header>

          <q-page-container>
            <q-page>
              <q-scroll-area style="height: 581px">
                <div>
                  <div
                    class="row q-mt-sm"
                    style="height: 86px; font-size: 10px"
                  >
                    <div class="col-4">
                      <q-img
                        src="../../assets/images/logo-hybrid-technologies.svg"
                        fit="cover"
                      />
                    </div>
                    <div class="col-8">
                      <div>
                        Human Resources Department - Hybrid Technologies Co.,
                        Ltd.
                      </div>
                      <div>
                        Address: 9th Floor, Viet A Tower, Duy Tan Str., Cau Giay
                        Dist., Hanoi., Viet Nam 19F, Ichigo Shinkawa Building,
                        2-22-1 Shinkawa, Chuo-ku, Tokyo 104-0033 Japan
                      </div>
                      <div>
                        Website:
                        <span class="text-cyan-7"
                          >https://hybrid-technologies.co.jp/</span
                        >
                      </div>
                    </div>
                  </div>

                  <div class="row">
                    <div class="col-9">
                      <div
                        class="q-pl-md bg-cyan-3 flex items-center height-view text-uppercase"
                      >
                        <span class="text-bold"
                          >{{ viewObj.firstName }} {{ viewObj.lastName }}</span
                        >
                      </div>

                      <div class="row height-view-sub">
                        <div
                          class="col-4 q-pl-md flex items-center border-right"
                        >
                          <span class="text-bold">Gender</span>
                        </div>
                        <div class="col-8 q-pl-md flex items-center">
                          {{ converGender(viewObj.gender) }}
                        </div>
                      </div>

                      <div class="row height-view-sub">
                        <div
                          class="col-4 q-pl-md flex items-center border-right text-bold"
                        >
                          Date of Birth
                        </div>
                        <div class="col-8 q-pl-md flex items-center">
                          {{ convertDateTimeToDate(viewObj.dateOfBirth) }}
                        </div>
                      </div>
                    </div>

                    <div class="col-3 border-image-view">
                      <q-img :src="viewObj.avatar.original" fit="cover" />
                    </div>
                  </div>

                  <div
                    class="q-pl-md bg-cyan-3 height-view flex items-center text-bold"
                  >
                    PROFESSIONAL OVERVIEW
                  </div>
                  <div class="q-pa-md flex items-center height-view-sub">
                    {{ viewObj.description }}
                  </div>

                  <div
                    class="q-pl-md bg-cyan-3 height-view flex items-center text-bold"
                    v-show="viewObj.orderIndex.includes(1)"
                  >
                    SKILLS
                  </div>

                  <div
                    class="row height-view-sub"
                    v-show="viewObj.orderIndex.includes(1)"
                    v-for="(skill, index) in viewObj.categoryPerson"
                    :key="index"
                  >
                    <div
                      class="col-4 flex items-center q-pl-md border-right text-bold"
                    >
                      {{ skill.categoryName }}
                    </div>
                    <div class="col-8 flex items-center q-pl-md">
                      <span>
                        <span
                          v-for="(technology, i) in skill.technologies"
                          :key="i"
                        >
                          {{ technology.name
                          }}<span v-show="i != skill.technologies.length - 1"
                            >,
                          </span>
                        </span>
                      </span>
                    </div>
                  </div>

                  <div
                    class="q-pl-md bg-cyan-3 height-view flex items-center text-bold"
                    v-show="viewObj.orderIndex.includes(3)"
                  >
                    WORKING HISTORY
                  </div>

                  <div
                    class="row height-view-sub"
                    v-show="viewObj.orderIndex.includes(3)"
                  >
                    <div
                      class="col-1 q-pl-md flex items-center border-right text-bold"
                    >
                      NO.
                    </div>
                    <div
                      class="col q-pl-md flex items-center border-right text-bold"
                    >
                      PREIOD
                    </div>
                    <div
                      class="col q-pl-md flex items-center border-right text-bold"
                    >
                      COMPANY
                    </div>
                    <div class="col q-pl-md flex items-center text-bold">
                      JOB TITTLE
                    </div>
                  </div>

                  <div
                    class="row height-view-sub"
                    v-show="viewObj.orderIndex.includes(3)"
                    v-for="(workHistory, index) in viewObj.workHistory"
                    :key="index"
                  >
                    <div class="col-1 q-pl-md flex items-center border-right">
                      {{ index + 1 }}
                    </div>
                    <div class="col q-pl-md flex items-center border-right">
                      <span
                        >{{
                          convertDateTimeToDate(
                            workHistory.startDate,
                            "MMM YYYY"
                          )
                        }}
                        -
                        {{
                          workHistory.endDate
                            ? convertDateTimeToDate(
                                workHistory.endDate,
                                "MMM YYYY"
                              )
                            : "Now"
                        }}</span
                      >
                    </div>
                    <div class="col q-pl-md flex items-center border-right">
                      {{ workHistory.companyName }}
                    </div>
                    <div class="col q-pl-md flex items-center">
                      {{ workHistory.position }}
                    </div>
                  </div>

                  <div
                    class="q-pl-md bg-cyan-3 height-view flex items-center text-bold"
                    v-show="viewObj.orderIndex.includes(4)"
                  >
                    EDUCATION
                  </div>

                  <div
                    class="education-view column q-pa-md"
                    v-show="viewObj.orderIndex.includes(4)"
                    v-for="(education, index) in viewObj.education"
                    :key="index"
                  >
                    <div class="flex items-center col">
                      <span class="text-bold"
                        >{{
                          convertDateTimeToDate(education.startDate, "YYYY")
                        }}
                        -
                        {{
                          education.endDate
                            ? convertDateTimeToDate(
                                education.endDate,
                                "MMM YYYY"
                              )
                            : "Now"
                        }}
                        | {{ education.collegeName }}</span
                      >
                    </div>
                    <div class="flex items-center col">
                      <span
                        ><span class="text-bold">Major: </span
                        >{{ education.major }}</span
                      >
                    </div>
                  </div>

                  <div
                    class="q-pl-md bg-cyan-3 height-view flex items-center text-bold"
                    v-show="viewObj.orderIndex.includes(5)"
                  >
                    CERTIFICATION
                  </div>

                  <div
                    class="height-view-sub q-pl-md flex items-center"
                    v-show="viewObj.orderIndex.includes(5)"
                    v-for="(certificate, index) in viewObj.certificate"
                    :key="index"
                  >
                    <span class="text-bold"
                      >{{
                        convertDateTimeToDate(certificate.startDate, "YYYY")
                      }}
                      | {{ certificate.name }}</span
                    >
                  </div>

                  <div
                    class="q-pl-md bg-cyan-3 height-view flex items-center text-bold"
                    v-show="viewObj.orderIndex.includes(2)"
                  >
                    PROJECTS
                  </div>

                  <div
                    class="row height-view-sub"
                    v-show="viewObj.orderIndex.includes(2)"
                  >
                    <div
                      class="col-1 flex items-center q-pl-md border-right text-bold"
                    >
                      NO.
                    </div>
                    <div
                      class="col flex items-center q-pl-md border-right text-bold"
                    >
                      PREIOD
                    </div>
                    <div
                      class="col flex items-center q-pl-md border-right text-bold"
                    >
                      POSITION
                    </div>
                    <div class="col-5 flex items-center q-pl-md text-bold">
                      DESCRIPTION
                    </div>
                  </div>

                  <div
                    class="row project-view"
                    v-show="viewObj.orderIndex.includes(2)"
                    v-for="(project, index) in viewObj.project"
                    :key="index"
                  >
                    <div class="col-1 q-pl-md flex items-center border-right">
                      {{ index + 1 }}
                    </div>
                    <div class="col q-pl-md flex items-center border-right">
                      {{ convertDateTimeToDate(project.startDate, "MMM YYYY") }}
                      -
                      {{
                        project.endDate
                          ? convertDateTimeToDate(project.endDate, "MMM YYYY")
                          : "Now"
                      }}
                    </div>
                    <div class="col q-pl-md flex items-center border-right">
                      {{ project.position }}
                    </div>
                    <div class="col-5">
                      <div
                        class="q-pl-md project-bottom-view flex items-center project-height-view"
                      >
                        <span class="text-bold">{{ project.name }}</span>
                      </div>
                      <div
                        class="q-pl-md project-bottom-view flex items-center project-height-view"
                      >
                        <span
                          ><span class="text-bold">Description: </span
                          >{{ project.description }}</span
                        >
                      </div>
                      <div
                        class="q-pl-md project-bottom-view flex items-center project-height-view"
                      >
                        <span
                          ><span class="text-bold">Responsibilities: </span
                          >{{ project.responsibilities }}</span
                        >
                      </div>
                      <div
                        class="q-pl-md project-bottom-view flex items-center project-height-view"
                      >
                        <span
                          ><span class="text-bold">Team size: </span
                          >{{ project.teamSize }}</span
                        >
                      </div>
                      <div
                        class="q-pl-md flex items-center project-height-view"
                      >
                        <span
                          ><span class="text-bold">Technologies used: </span
                          ><span
                            v-for="(technology, i) in project.technologies"
                            :key="i"
                          >
                            {{ technology.name
                            }}<span
                              v-show="i != project.technologies.length - 1"
                              >,
                            </span>
                          </span></span
                        >
                      </div>
                    </div>
                  </div>
                </div>
              </q-scroll-area>
            </q-page>
          </q-page-container>

          <q-footer class="bg-accent text-white">
            <q-toolbar class="flex flex-center">
              <q-btn
                v-show="false"
                dense
                color="primary"
                label="Download"
                class="q-px-lg"
              />
            </q-toolbar>
          </q-footer>
        </q-layout>
      </q-dialog>

      <div
        class="table-component full-height full-width flex flex-center q-px-md"
      >
        <div class="new-item q-mb-md flex justify-end full-width">
          <div v-show="!filter.available">
            <q-input
              dense
              readonly
              clearable
              standout
              v-model="filter.lastDay"
              type="text"
              placeholder="YYYY-MM-DD"
              stack-label
              label="Day finish project:"
              label-color="white"
              bg-color="primary"
              input-class="text-white"
              hide-bottom-space
            >
              <template v-slot:append>
                <q-icon name="event" class="cursor-pointer" color="white">
                  <q-popup-proxy
                    cover
                    transition-show="scale"
                    transition-hide="scale"
                  >
                    <q-date
                      v-model="filter.lastDay"
                      mask="YYYY-MM-DD"
                      @update:model-value="getEmployeeWithFilter(false)"
                    >
                      <div class="row items-center justify-end">
                        <q-btn
                          v-close-popup
                          @click="clearLastDay"
                          label="Clear"
                          color="primary"
                          flat
                        />

                        <q-btn
                          v-close-popup
                          label="Close"
                          color="primary"
                          flat
                        />
                      </div>
                    </q-date>
                  </q-popup-proxy>
                </q-icon>
              </template>
            </q-input>
          </div>
          <q-btn
            v-show="getRole == 'editor-qtns'"
            class="q-ml-md"
            @click="openInsert"
            color="primary"
            label="New employee"
            unelevated
          />
        </div>

        <q-table
          class="table-content"
          :rows="listEmployee"
          :columns="headerTable"
          row-key="id"
          flat
          bordered
          dark
          :loading="loadingData"
          v-model:pagination="pagination"
          :rows-per-page-options="[5, 10, 15, 20]"
          @request="getEmployeeWithFilter"
        >
          <template v-slot:header-cell-staffId="props">
            <q-th :props="props">
              <div :style="`min-width: ${widthOfStaffId}`">
                <q-input
                  @update:model-value="getEmployeeWithFilter(false)"
                  dark
                  dense
                  standout
                  v-model="filter.staffId"
                  input-class="text-right"
                  :label="labelNameFocus[0]"
                  :label-color="labelColorFocus[0]"
                  debounce="300"
                  @focus="
                    labelColorFocus[0] = 'black';
                    labelNameFocus[0] = 'Search by staff id';
                    widthOfStaffId = '154px';
                  "
                  @blur="
                    labelColorFocus[0] = 'white';
                    labelNameFocus[0] = props.col.label;
                    widthOfStaffId = '100px';
                  "
                >
                  <template v-slot:append>
                    <q-icon v-if="!filter.staffId" name="search" />
                    <q-icon
                      v-else
                      name="clear"
                      class="cursor-pointer"
                      @click="
                        filter.staffId = '';
                        getEmployeeWithFilter(false);
                      "
                    />
                  </template>
                </q-input>
              </div>
            </q-th>
          </template>

          <template v-slot:header-cell-position="props">
            <q-th :props="props">
              <div :style="`width: ${widthOfPosition};`">
                <q-select
                  @update:model-value="getEmployeeWithFilter(false)"
                  dense
                  standout
                  dark
                  clearable
                  v-model="filter.positionId"
                  :options="tempListPosition"
                  :label="props.col.label"
                  option-value="id"
                  option-label="name"
                  emit-value
                  map-options
                  options-selected-class="text-accent"
                  @filter="filterPosition"
                  input-debounce="100"
                  fill-input
                  hide-selected
                  use-input
                  :label-color="labelColorFocus[1]"
                  @focus="
                    labelColorFocus[1] = 'black';
                    widthOfPosition = '160px';
                  "
                  @blur="
                    labelColorFocus[1] = 'white';
                    widthOfPosition = !filter.positionId ? '120px' : '160px';
                  "
                  ><template v-slot:no-option>
                    <q-item>
                      <q-item-section class="text-grey">
                        No results
                      </q-item-section>
                    </q-item>
                  </template>
                </q-select>
              </div>
            </q-th>
          </template>

          <template v-slot:header-cell-fullName="props">
            <q-th :props="props">
              <div :style="`min-width: ${widthOfFullName};`">
                <q-input
                  @update:model-value="getEmployeeWithFilter(false)"
                  dark
                  dense
                  standout
                  debounce="400"
                  v-model="filter.firstName"
                  input-class="text-right"
                  :label="labelNameFocus[2]"
                  :label-color="labelColorFocus[2]"
                  @focus="
                    labelColorFocus[2] = 'black';
                    labelNameFocus[2] = 'Search by first name';
                    widthOfFullName = '170px';
                  "
                  @blur="
                    labelColorFocus[2] = 'white';
                    labelNameFocus[2] = props.col.label;
                    widthOfFullName = '130px';
                  "
                >
                  <template v-slot:append>
                    <q-icon v-if="!filter.firstName" name="search" />
                    <q-icon
                      v-else
                      name="clear"
                      class="cursor-pointer"
                      @click="
                        filter.firstName = '';
                        getEmployeeWithFilter(false);
                      "
                    />
                  </template>
                </q-input>
              </div>
            </q-th>
          </template>

          <template v-slot:header-cell-available="props">
            <q-th :props="props">
              <div>
                <q-checkbox
                  left-label
                  dense
                  dark
                  v-model="filter.available"
                  color="accent"
                  @update:model-value="getEmployeeWithFilter(false)"
                  label="Available"
                />
              </div>
            </q-th>
          </template>

          <template v-slot:header-cell-skill="props">
            <q-th :props="props">
              <div :style="`width: ${widthOfSkill};`">
                <q-select
                  dense
                  standout
                  dark
                  max-values="3"
                  clearable
                  multiple
                  use-chips
                  use-input
                  input-debounce="200"
                  v-model="filter.technologyId"
                  :options="tempListSkill"
                  :label="labelNameFocus[3]"
                  option-value="id"
                  option-label="name"
                  emit-value
                  map-options
                  options-selected-class="text-accent"
                  :label-color="labelColorFocus[3]"
                  @update:model-value="getEmployeeWithFilter(false)"
                  @filter="filterSkill"
                  @focus="
                    labelColorFocus[3] = 'black';
                    labelNameFocus[3] = 'Search by skill';
                    widthOfSkill = '250px';
                  "
                  @blur="
                    labelColorFocus[3] = 'white';
                    labelNameFocus[3] = props.col.label;
                    widthOfSkill = !filter?.technologyId?.length
                      ? '150px'
                      : '250px';
                  "
                  ><template v-slot:no-option>
                    <q-item>
                      <q-item-section class="text-grey">
                        No results
                      </q-item-section>
                    </q-item>
                  </template>
                </q-select>
              </div>
            </q-th>
          </template>

          <template v-slot:body-cell-avatar="props">
            <q-td :props="props">
              <q-avatar>
                <img :src="props.value" />
              </q-avatar>
            </q-td>
          </template>

          <template v-slot:body-cell-action="props">
            <q-td :props="props">
              <div>
                <q-btn
                  style="width: 60px"
                  dense
                  color="white"
                  text-color="black"
                  label="Edit"
                  @click="openEdit(props.value)"
                />
              </div>
              <div class="q-mt-sm">
                <q-btn
                  style="width: 60px"
                  dense
                  color="negative"
                  label="delete"
                  @click="
                    idDelete = props.value;
                    showDelete = true;
                  "
                />
              </div>
            </q-td>
          </template>

          <template v-slot:body-cell-available="props">
            <q-td :props="props">
              <q-icon
                v-show="props.value"
                name="fas fa-check"
                color="positive"
                size="16px"
              />
            </q-td>
          </template>

          <template v-slot:body-cell-fullName="props">
            <q-td :props="props">
              {{ showName(props.value) }}
            </q-td>
          </template>

          <template v-slot:body-cell-skill="props">
            <q-td :props="props">
              <div v-if="props.value?.length">
                <div
                  v-for="(item, index) in props.value.slice(0, 3)"
                  :key="index"
                >
                  <q-badge :color="index % 2 == 0 ? 'blue-10' : 'teal-8'">
                    <span style="font-size: 14px"
                      >{{ props?.value[index].categoryName }}:</span
                    >
                  </q-badge>
                  <span>
                    {{
                      props?.value[index]?.technologies[0]?.name
                        ? ` ${props?.value[index]?.technologies[0]?.name},`
                        : ""
                    }}{{
                      props?.value[index]?.technologies[1]?.name
                        ? ` ${props?.value[index]?.technologies[1]?.name},`
                        : ""
                    }}{{
                      props?.value[index]?.technologies[2]?.name
                        ? ` ${props?.value[index]?.technologies[2]?.name},`
                        : ""
                    }}
                    <span v-show="props?.value[index]?.technologies.length"
                      >...</span
                    >
                  </span>
                </div>
              </div>
            </q-td>
          </template>

          <template v-slot:loading>
            <q-inner-loading showing color="primary" />
          </template>
        </q-table>
      </div>
    </div>
  </q-page>
</template>

<script>
import { defineComponent } from "vue";
import { mapActions, mapGetters } from "vuex";
import { useQuasar, date } from "quasar";
import { api } from "src/boot/axios";
import EditEmployee from "src/pages/Employee/EditEmployee.vue";
import NewEmployee from "src/pages/Employee/NewEmployee.vue";

export default defineComponent({
  name: "List Employee",

  components: {
    "edit-employee": EditEmployee,
    "new-employee": NewEmployee,
  },

  data() {
    return {
      labelColorFocus: ["white", "white", "white", "white"],
      labelNameFocus: ["Staff ID", "", "Full Name", "Skills"],

      widthOfStaffId: "100px",
      widthOfPosition: "120px",
      widthOfFullName: "130px",
      widthOfSkill: "150px",

      loadingData: false,

      showDelete: false,
      idDelete: null,
      deleteProcess: false,

      showEdit: false,
      editObj: null,
      statusUpdate: false,

      showInsert: false,
      statusInsert: false,

      showView: false,
      viewObj: null,

      filter: {
        staffId: null,
        positionId: null,
        firstName: null,
        available: false,
        lastDay: null,
        technologyId: [],
      },

      pagination: {
        page: 1,
        rowsPerPage: 10,
        rowsNumber: 10, // is total records

        firstPage: 1,
        lastPage: null,
        nextPage: null,
        previousPage: null,
        totalPages: 0,
      },

      tempListPosition: [],
      listPosition: [],
      tempListSkill: [],
      listSkill: [],
      listEmployee: [],
      headerTable: [
        {
          name: "index",
          label: "#",
          align: "center",
          field: "index",
        },
        {
          name: "staffId",
          align: "left",
          label: "Staff ID",
          field: "staffId",
        },
        {
          name: "fullName",
          align: "left",
          label: "Full Name",
          field: (row) => `${row.firstName} ${row.lastName}`,
        },
        {
          name: "avatar",
          align: "center",
          label: "",
          field: (row) => row.avatar.thumbnail,
        },
        {
          name: "position",
          align: "left",
          label: "Position",
          field: (row) => row.position.name,
        },
        {
          name: "available",
          align: "center",
          label: "Available",
          field: (row) => row.available,
        },
        {
          name: "skill",
          align: "left",
          label: "Skills",
          field: (row) => row.categoryPerson,
        },
        {
          name: "action",
          align: "center",
          label: "Actions",
          field: (row) => row.id,
        },
      ],
    };
  },
  methods: {
    ...mapActions("auth", ["useRefreshToken", "validateToken"]),

    openInsert() {
      this.showInsert = true;
    },
    closeInsertDialog() {
      this.showInsert = false;
      this.statusInsert = false;
    },
    saveInsert() {
      this.statusInsert = true;
    },
    async insertReceive(value) {
      if (value) {
        this.statusInsert = false;

        let result = await this.getEmployeeById(value);
        if (this.listEmployee.length >= 10) this.listEmployee.pop();
        this.listEmployee.unshift(result);
        this.listEmployee.forEach((row, index) => {
          row.index = index + 1;
        });

        // Update pagination
        this.pagination.rowsNumber = this.pagination.rowsNumber + 1;
      } else {
        this.statusInsert = false;
      }
    },
    async getEmployee() {
      try {
        this.loadingData = true;

        // Request API
        let result = await api
          .get(`/api/v1/person?page=1&pageSize=10`)
          .then((response) => {
            return response.data;
          })
          .catch(function (error) {
            // Checking if throw error
            if (error.response) {
              // Server response
              return error.response.data;
            } else {
              // Server not working
              let temp = { success: false, message: ["Server Error!"] };
              return temp;
            }
          });

        if (result.success) {
          this.mappingPagination(result);
        } else {
          this.$q.notify({
            type: "negative",
            message: result.message[0],
          });
        }
      } finally {
        this.loadingData = false;
      }
    },
    async getEmployeeWithFilter(props) {
      try {
        this.loadingData = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        let { page, rowsPerPage } = props
          ? props.pagination
          : { page: 1, rowsPerPage: this.pagination.rowsPerPage };

        if (!this.filter.lastDay) this.filter.lastDay = null;

        // Request API
        let result = await api
          .post(
            `/api/v1/person/pagination?page=${page}&pageSize=${rowsPerPage}`,
            this.filter
          )
          .then((response) => {
            return response.data;
          })
          .catch(function (error) {
            // Checking if throw error
            if (error.response) {
              // Server response
              return error.response.data;
            } else {
              // Server not working
              let temp = { success: false, message: ["Server Error!"] };
              return temp;
            }
          });

        if (result.success) {
          this.mappingPagination(result);
        } else {
          this.$q.notify({
            type: "negative",
            message: result.message[0],
          });
        }
      } finally {
        this.loadingData = false;
      }
    },
    async getPosition() {
      // Request API
      let result = await api
        .get("/api/v1/position")
        .then((response) => {
          return response.data;
        })
        .catch(function (error) {
          // Checking if throw error
          if (error.response) {
            // Server response
            return error.response.data;
          } else {
            // Server not working
            let temp = { success: false, message: ["Server Error!"] };
            return temp;
          }
        });

      if (result.success) {
        this.listPosition = result.resource;
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async findTechnology(keyword, seedValue) {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      // Request API
      let result = await api
        .get(
          `/api/v1/technology/search?filterName=${
            !keyword ? "" : keyword.trim()
          }`
        )
        .then((response) => {
          return response.data;
        })
        .catch(function (error) {
          // Checking if throw error
          if (error.response) {
            // Server response
            return error.response.data;
          } else {
            // Server not working
            let temp = { success: false, message: ["Server Error!"] };
            return temp;
          }
        });

      if (result.success) {
        this.tempListSkill = result.resource;
        if (seedValue) this.listSkill = result.resource;
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    mappingPagination(resource) {
      this.listEmployee = resource.resource;

      this.listEmployee.forEach((row, index) => {
        row.index = index + 1;
      });

      this.pagination.page = resource.page;
      this.pagination.rowsPerPage = resource.pageSize;
      this.pagination.rowsNumber = resource.totalRecords;

      this.pagination.firstPage = resource.firstPage;
      this.pagination.lastPage = resource.lastPage;
      this.pagination.nextPage = resource.nextPage;
      this.pagination.previousPage = resource.previousPage;
      this.pagination.totalPages = resource.totalPages;
    },
    showName(text) {
      if (text.length > 20) return `${text.slice(0, 20)}...`;
      else return text;
    },
    filterPosition(val, update, abort) {
      update(() => {
        if (!val) {
          this.tempListPosition = this.listPosition.slice(0, 5);
        } else {
          let needle = val.toLowerCase();
          this.tempListPosition = this.listPosition.filter(
            (v) => v.name.toLowerCase().indexOf(needle) > -1
          );
        }
      });
    },
    filterSkill(val, update, abort) {
      update(async () => {
        if (val.length < 2) this.tempListSkill = this.listSkill;
        else await this.findTechnology(val, false);
      });
    },
    async deleteEmployee() {
      try {
        this.deleteProcess = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        // Request API
        let result = await api
          .delete(`/api/v1/person/${this.idDelete}`)
          .then((response) => {
            return response.data;
          })
          .catch(function (error) {
            // Checking if throw error
            if (error.response) {
              // Server response
              return error.response.data;
            } else {
              // Server not working
              let temp = { success: false, message: ["Server Error!"] };
              return temp;
            }
          });

        if (result.success) {
          await this.getEmployeeWithFilter(false);

          this.$q.notify({
            type: "positive",
            message: "Successfully deleted!",
          });
        } else {
          this.$q.notify({
            type: "negative",
            message: result.message[0],
          });
        }
      } finally {
        this.deleteProcess = false;
        this.showDelete = false;
      }
    },
    openEdit(id) {
      this.editObj = this.listEmployee.find((x) => x.id == id);
      this.showEdit = true;
    },
    saveUpdate() {
      this.statusUpdate = true;
    },
    async updateReceive(value) {
      if (value) {
        let result = await this.getEmployeeById(this.editObj.id);

        this.statusUpdate = false;
        this.showEdit = false;

        let index = this.listEmployee.findIndex((x) => x.id == this.editObj.id);
        let numberIndex = this.listEmployee[index].index;
        this.listEmployee[index] = Object.assign({}, result);
        this.listEmployee[index].index = numberIndex;
        this.editObj = null;
      } else {
        this.statusUpdate = false;
      }
    },
    async closeEditDialog() {
      let result = await this.getEmployeeById(this.editObj.id);

      let index = this.listEmployee.findIndex((x) => x.id == this.editObj.id);
      let numberIndex = this.listEmployee[index].index;
      this.listEmployee[index] = Object.assign({}, result);
      this.listEmployee[index].index = numberIndex;
      this.editObj = null;

      this.statusUpdate = false;
      this.showEdit = false;
    },
    async getEmployeeById(id) {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      // Request API
      let result = await api
        .get(`/api/v1/person/${id}`)
        .then((response) => {
          return response.data;
        })
        .catch(function (error) {
          // Checking if throw error
          if (error.response) {
            // Server response
            return error.response.data;
          } else {
            // Server not working
            let temp = { success: false, message: ["Server Error!"] };
            return temp;
          }
        });

      return result?.resource;
    },
    openViewDialog(id) {
      this.viewObj = this.listEmployee.find((x) => x.id == id);
      this.showView = true;
    },
    converGender(value) {
      if (value == 1) return "Male";
      if (value == 2) return "Female";
      if (value == 3) return "Sexless";
    },
    convertDateTimeToDate(dateTime, stringFormat = "YYYY-MM-DD") {
      return date.formatDate(dateTime, stringFormat);
    },
    async clearLastDay() {
      this.filter.lastDay = null;

      await this.getEmployeeWithFilter(false);
    },
  },
  computed: {
    ...mapGetters("auth", ["getRole", "getInformation"]),

    getNameDelete() {
      let tempEmployee = this.listEmployee.filter((x) => x.id == this.idDelete);

      let name = tempEmployee[0]?.firstName;

      return name ? this.showName(name) : "";
    },
  },
  async created() {
    let isValid = await this.validateToken();
    if (!isValid) this.$router.replace("/login");

    await Promise.all([
      this.getEmployee(),
      this.getPosition(),
      this.findTechnology(false, true),
    ]);
  },
  mounted() {
    const $q = useQuasar();
  },
});
</script>

<style lang="scss" scoped>
.container {
  position: relative;

  .table-component {
    position: relative;

    .table-content {
      /* height or max-height is important */
      height: 600px;
      width: 100%;
    }
  }
}

.height-view {
  min-height: 52px;
  border: 1px solid $primary;
  border-bottom: 0;
}

.height-view-sub {
  min-height: 44px;
  border: 1px solid $primary;
  border-bottom: 0;
}

.border-right {
  border-right: 1px solid $primary;
}

.education-view {
  min-height: 71px;
  border: 1px solid $primary;
  border-bottom: 0;
}

.border-image-view {
  border-top: 1px solid $primary;
  border-right: 1px solid $primary;
}

.project-view {
  border: 1px solid $primary;
  border-bottom: 0;
}

.project-bottom-view {
  border-bottom: 1px solid $primary;
}

.project-height-view {
  min-height: 30px;
}
</style>

<style lang="scss">
.table-content {
  .q-table__top,
  .q-table__bottom,
  thead tr:first-child th {
    /* bg color is important for th; just specify one */
    background-color: $accent !important;
  }

  thead tr th {
    position: sticky;
    z-index: 99;
    font-size: 14px !important;
    font-family: Poppins-Medium !important;
  }

  tbody tr td {
    font-size: 14px !important;
  }

  thead tr:first-child th {
    top: 0;
  }

  /* this is when the loading indicator appears */
  &.q-table--loading thead tr:last-child th {
    /* height of all previous header rows */
    top: 0;
  }
}

/* Scroll bar */
::-webkit-scrollbar {
  width: 10px;
  height: 10px;
}

::-webkit-scrollbar-thumb {
  background: $accent;
}

::-webkit-scrollbar-corner {
  background: $grey;
}
</style>
