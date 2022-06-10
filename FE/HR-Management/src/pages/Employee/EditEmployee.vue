<template>
  <q-page class="fit">
    <div class="row container">
      <div class="col-6 left-side flex flex-center">
        <div class="fit q-py-lg">
          <q-scroll-area style="height: 532px">
            <div class="avatar flex flex-center column">
              <q-avatar size="100px">
                <img :src="imageURL" />
              </q-avatar>
              <div class="flex row no-wrap" style="width: 300px">
                <div class="flex flex-center q-mr-md q-pt-sm">
                  <q-btn
                    round
                    unelevated
                    color="primary"
                    size="10px"
                    icon="restart_alt"
                    @click="resetAvatar"
                    ><q-tooltip anchor="top middle" self="bottom middle"
                      >Reset Avatar</q-tooltip
                    >
                  </q-btn>
                </div>
                <q-file
                  class="q-mt-sm cursor-pointer"
                  dense
                  tabindex="1"
                  clearable
                  standout
                  input-style="width: 150px;"
                  max-total-size="5242880"
                  :display-value="
                    imageFile ? 'Image is uploaded' : 'Upload image here!'
                  "
                  v-model="imageFile"
                  accept=".jpg, .png, .gif, .bmp, image/*"
                  @update:model-value="previewImage"
                  @clear="clearTempImage"
                  @rejected="onRejected"
                >
                  <template v-slot:prepend>
                    <q-icon name="attach_file" />
                  </template>
                </q-file>
              </div>
            </div>

            <div class="first-name q-mt-lg q-px-lg">
              <q-input
                ref="firstNameRef"
                tabindex="2"
                standout
                clearable
                maxlength="500"
                v-model="employeeInfor.firstName"
                type="text"
                label="First Name:"
                :label-color="labelColorFocus[0]"
                @focus="labelColorFocus[0] = 'white'"
                @blur="labelColorFocus[0] = ''"
                :rules="[(val) => !!val || 'First Name is required']"
                hide-bottom-space
              >
              </q-input>
            </div>

            <div class="last-name q-mt-sm q-px-lg">
              <q-input
                ref="lastNameRef"
                tabindex="3"
                standout
                clearable
                maxlength="500"
                v-model="employeeInfor.lastName"
                type="text"
                label="Last Name:"
                :label-color="labelColorFocus[1]"
                @focus="labelColorFocus[1] = 'white'"
                @blur="labelColorFocus[1] = ''"
                :rules="[(val) => !!val || 'Last Name is required']"
                hide-bottom-space
              ></q-input>
            </div>

            <div class="dob q-mt-sm q-px-lg">
              <q-input
                ref="dobRef"
                tabindex="4"
                standout
                maxlength="500"
                v-model="employeeInfor.dateOfBirth"
                type="text"
                placeholder="YYYY-MM-DD"
                mask="####-##-##"
                stack-label
                label="Date Of Birth:"
                :label-color="labelColorFocus[2]"
                @focus="labelColorFocus[2] = 'white'"
                @blur="labelColorFocus[2] = ''"
                :rules="[
                  (val) => !!val || 'DoB is required',
                  (val) => validateDoB(val) || 'DoB is invalid',
                ]"
                hide-bottom-space
              >
                <template v-slot:append>
                  <q-icon name="event" class="cursor-pointer">
                    <q-popup-proxy
                      cover
                      transition-show="scale"
                      transition-hide="scale"
                    >
                      <q-date
                        v-model="employeeInfor.dateOfBirth"
                        mask="YYYY-MM-DD"
                      >
                        <div class="row items-center justify-end">
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

            <div class="gender q-mt-sm q-px-lg">
              <q-select
                standout
                ref="genderRef"
                tabindex="5"
                v-model="employeeInfor.gender"
                :options="listGender"
                label="Gender:"
                option-value="id"
                option-label="name"
                emit-value
                map-options
                options-selected-class="text-accent"
                :rules="[(val) => !!val || 'Gender is required']"
                hide-bottom-space
                :label-color="labelColorFocus[3]"
                @focus="labelColorFocus[3] = 'white'"
                @blur="labelColorFocus[3] = ''"
              >
              </q-select>
            </div>

            <div class="description q-mt-sm q-px-lg">
              <q-input
                ref="descriptionRef"
                standout
                tabindex="6"
                clearable
                maxlength="500"
                v-model="employeeInfor.description"
                type="text"
                autogrow
                label="Description:"
                :label-color="labelColorFocus[4]"
                @focus="labelColorFocus[4] = 'white'"
                @blur="labelColorFocus[4] = ''"
                hide-bottom-space
              ></q-input>
            </div>

            <div class="position q-mt-sm q-px-lg">
              <q-select
                standout
                ref="positionRef"
                tabindex="7"
                v-model="employeeInfor.positionId"
                :options="tempListPosition"
                label="Position:"
                option-value="id"
                option-label="name"
                emit-value
                map-options
                options-selected-class="text-accent"
                @filter="filterPosition"
                fill-input
                hide-selected
                use-input
                :rules="[(val) => !!val || 'Position is required']"
                hide-bottom-space
                :label-color="labelColorFocus[5]"
                @focus="labelColorFocus[5] = 'white'"
                @blur="labelColorFocus[5] = ''"
                ><template v-slot:no-option>
                  <q-item>
                    <q-item-section class="text-grey">
                      No results
                    </q-item-section>
                  </q-item>
                </template>
              </q-select>
            </div>

            <div class="email q-mt-sm q-px-lg">
              <q-input
                ref="emailRef"
                tabindex="8"
                standout
                clearable
                maxlength="500"
                v-model="employeeInfor.email"
                type="email"
                label="Email:"
                :label-color="labelColorFocus[6]"
                @focus="labelColorFocus[6] = 'white'"
                @blur="labelColorFocus[6] = ''"
                hide-bottom-space
              >
              </q-input>
            </div>

            <div class="phone q-mt-sm q-px-lg">
              <q-input
                ref="phoneRef"
                mask="#########################"
                standout
                clearable
                tabindex="9"
                maxlength="25"
                v-model="employeeInfor.phone"
                type="text"
                label="Phone:"
                :label-color="labelColorFocus[7]"
                @focus="labelColorFocus[7] = 'white'"
                @blur="labelColorFocus[7] = ''"
                hide-bottom-space
              >
              </q-input>
            </div>

            <div class="work q-mt-sm q-px-lg">
              <q-select
                standout
                ref="workRef"
                tabindex="10"
                clearable
                v-model="employeeInfor.groupId"
                :options="tempListWork"
                label="Current Project:"
                option-value="id"
                option-label="name"
                emit-value
                map-options
                options-selected-class="text-accent"
                @filter="filterWork"
                fill-input
                hide-selected
                use-input
                hide-bottom-space
                :label-color="labelColorFocus[8]"
                @focus="labelColorFocus[8] = 'white'"
                @blur="labelColorFocus[8] = ''"
                ><template v-slot:no-option>
                  <q-item>
                    <q-item-section class="text-grey">
                      No results
                    </q-item-section>
                  </q-item>
                </template>
              </q-select>
            </div>
          </q-scroll-area>
        </div>
      </div>
      <div class="col-6 right-side">
        <div class="fit q-pa-lg">
          <q-card>
            <q-tabs
              v-model="tab"
              dense
              outside-arrows
              mobile-arrows
              class="text-grey"
              active-color="primary"
              indicator-color="primary"
              align="justify"
              narrow-indicator
            >
              <q-tab
                v-for="item in tabModel"
                :key="item.id"
                :name="item.id"
                :label="item.name"
              />
            </q-tabs>

            <q-separator />

            <q-tab-panels v-model="tab" animated>
              <q-tab-panel name="1" class="tab-skill flex flex-center">
                <div class="full-width relative-position flex flex-center">
                  <q-btn-group rounded flat unelevated>
                    <q-btn
                      color="primary"
                      label="<"
                      @click="orderTabPrev()"
                      size="md"
                    />
                    <q-btn
                      :color="booleanTab(1) ? 'primary' : 'grey'"
                      label="Skill"
                      @click="toggleTab(1)"
                      size="md"
                    >
                      <q-tooltip anchor="top middle" self="center middle">
                        {{ booleanTab(1) ? "Hide" : "Show" }}
                      </q-tooltip></q-btn
                    >
                    <q-btn
                      color="primary"
                      label=">"
                      @click="orderTabNext()"
                      size="md"
                    />
                  </q-btn-group>

                  <q-btn
                    size="12px"
                    class="absolute-top-right"
                    icon="add"
                    color="primary"
                    round
                    unelevated
                    @click="openDialog(1)"
                  />
                </div>

                <div class="fit">
                  <q-scroll-area class="q-mt-md" style="height: 411px">
                    <q-card
                      v-for="(category, index) in listTempCategory"
                      :key="index"
                      bordered
                      class="inside"
                    >
                      <q-card-section horizontal class="row">
                        <q-card-section class="col-10">
                          <div class="q-mb-sm">
                            <q-badge
                              class="text-subtitle2"
                              :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            >
                              <span>{{ category?.categoryName }}:</span>
                            </q-badge>
                          </div>

                          <span
                            v-for="skill in category?.technologies"
                            :key="skill?.id"
                            class="q-mr-sm"
                          >
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                            >
                              {{ skill?.name }}
                            </q-badge>
                          </span>
                        </q-card-section>

                        <q-card-actions vertical class="col-2 justify-around">
                          <q-btn
                            @click="orderSkillPrev(category)"
                            flat
                            round
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            icon="keyboard_arrow_up"
                          />

                          <q-fab
                            class="q-my-sm"
                            icon="more_horiz"
                            direction="left"
                            padding="sm"
                            unelevated
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                          >
                            <q-fab-action
                              @click="editSkill(category)"
                              color="info"
                              icon="edit"
                              ><q-tooltip
                                anchor="top middle"
                                self="center middle"
                              >
                                Edit
                              </q-tooltip></q-fab-action
                            >
                            <q-fab-action
                              @click="deleteSkill(category)"
                              color="negative"
                              icon="delete"
                              ><q-tooltip
                                anchor="top middle"
                                self="center middle"
                              >
                                Delete
                              </q-tooltip></q-fab-action
                            >
                          </q-fab>

                          <q-btn
                            flat
                            @click="orderSkillNext(category)"
                            round
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            icon="keyboard_arrow_down"
                          />
                        </q-card-actions>
                      </q-card-section>
                    </q-card>
                  </q-scroll-area>
                </div>

                <q-dialog v-model="dialogToggle[1]" persistent>
                  <q-card style="width: 400px">
                    <q-card-section class="row items-center">
                      <span class="text-h6">{{
                        showEditBtn ? "Edit Skill" : "Add Skill"
                      }}</span>
                    </q-card-section>

                    <q-separator />

                    <q-card-section>
                      <div class="q-mt-sm">
                        <q-select
                          standout
                          ref="categoryRef"
                          v-model="tempCategoryPersonResource.categoryId"
                          :options="tempListCategory"
                          :label="labelNameFocusSkill[0]"
                          :option-disable="
                            (item) =>
                              getDisableCategory.some((x) => x == item.id)
                                ? true
                                : false
                          "
                          option-value="id"
                          option-label="name"
                          emit-value
                          map-options
                          options-selected-class="text-accent"
                          @filter="filterCategory"
                          @update:model-value="
                            this.tempCategoryPersonResource.technologies = []
                          "
                          fill-input
                          input-debounce="200"
                          hide-selected
                          use-input
                          :rules="[(val) => !!val || 'Position is required']"
                          lazy-rules="ondemand"
                          hide-bottom-space
                          :label-color="colorFocusSkill[0]"
                          @focus="
                            colorFocusSkill[0] = 'white';
                            labelNameFocusSkill[0] = 'Search by category';
                          "
                          @blur="
                            colorFocusSkill[0] = '';
                            labelNameFocusSkill[0] = 'Category:';
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

                      <div class="q-mt-sm">
                        <q-select
                          ref="skillRef"
                          standout
                          clearable
                          multiple
                          use-chips
                          max-values="̀50"
                          use-input
                          :disable="
                            tempCategoryPersonResource.categoryId ? false : true
                          "
                          v-model="tempCategoryPersonResource.technologies"
                          :options="tempListSkillCategory"
                          :label="labelNameFocusSkill[1]"
                          option-value="id"
                          option-label="name"
                          emit-value
                          map-options
                          options-selected-class="text-accent"
                          :label-color="colorFocusSkill[1]"
                          :rules="[(val) => val?.length || 'Skill is required']"
                          lazy-rules="ondemand"
                          @filter="filterSkillBelongWithCategory"
                          @focus="
                            colorFocusSkill[1] = 'white';
                            labelNameFocusSkill[1] = 'Search by skill';
                          "
                          @blur="
                            colorFocusSkill[1] = '';
                            labelNameFocusSkill[1] = 'Skill:';
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
                    </q-card-section>

                    <q-card-actions align="right">
                      <q-btn
                        flat
                        label="Cancel"
                        color="primary"
                        v-close-popup
                      />
                      <q-btn
                        flat
                        label="Add"
                        color="info"
                        @click="addSkill"
                        v-show="!showEditBtn"
                      />
                      <q-btn
                        flat
                        label="Save"
                        color="info"
                        @click="saveSkill"
                        v-show="showEditBtn"
                      />
                    </q-card-actions>
                  </q-card>
                </q-dialog>

                <q-dialog v-model="deleteToggle[1]" persistent>
                  <q-card>
                    <q-card-section class="row items-center">
                      <span class="text-h6"
                        >Delete {{ tempDeleteSkill.name }} category?</span
                      >
                    </q-card-section>

                    <q-separator />

                    <q-card-section>
                      <span>This can’t be undone and it will be removed.</span>
                    </q-card-section>

                    <q-card-actions align="right">
                      <q-btn
                        flat
                        v-close-popup
                        label="Cancel"
                        color="primary"
                      />
                      <q-btn
                        flat
                        label="Delete"
                        color="negative"
                        @click="saveDeleteSkill"
                      />
                    </q-card-actions>
                  </q-card>
                </q-dialog>
              </q-tab-panel>

              <q-tab-panel name="2" class="tab-project flex flex-center">
                <div class="full-width relative-position flex flex-center">
                  <q-btn-group rounded flat unelevated>
                    <q-btn
                      color="primary"
                      label="<"
                      @click="orderTabPrev()"
                      size="md"
                    />
                    <q-btn
                      :color="booleanTab(2) ? 'primary' : 'grey'"
                      label="Project"
                      @click="toggleTab(2)"
                      size="md"
                    >
                      <q-tooltip anchor="top middle" self="center middle">
                        {{ booleanTab(2) ? "Hide" : "Show" }}
                      </q-tooltip></q-btn
                    >
                    <q-btn
                      color="primary"
                      label=">"
                      @click="orderTabNext()"
                      size="md"
                    />
                  </q-btn-group>

                  <q-btn
                    size="12px"
                    class="absolute-top-right"
                    icon="add"
                    color="primary"
                    round
                    unelevated
                    @click="openDialog(2)"
                  />
                </div>

                <div class="fit">
                  <q-scroll-area class="q-mt-md" style="height: 411px">
                    <q-card
                      v-for="(project, index) in listTempProject"
                      :key="index"
                      bordered
                      class="inside"
                    >
                      <q-card-section horizontal class="row">
                        <q-card-section class="col-10">
                          <div class="q-mb-sm">
                            <q-badge
                              class="text-subtitle2"
                              :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            >
                              <span>{{ project?.name }}:</span>
                            </q-badge>
                          </div>

                          <div>
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                              >Position:</q-badge
                            >
                            {{ project.position }}
                          </div>

                          <div>
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                              >Responsibilities:</q-badge
                            >
                            {{ project.responsibilities }}
                          </div>

                          <div>
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                              >Start Date:</q-badge
                            >
                            {{ convertDateTimeToDate(project.startDate) }}
                          </div>

                          <div v-show="project.endDate">
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                              >End Date:</q-badge
                            >
                            {{ convertDateTimeToDate(project.endDate) }}
                          </div>
                        </q-card-section>

                        <q-card-actions vertical class="col-2 justify-around">
                          <q-btn
                            @click="orderProjectPrev(project)"
                            flat
                            round
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            icon="keyboard_arrow_up"
                          />

                          <q-fab
                            class="q-my-sm"
                            icon="more_horiz"
                            direction="left"
                            padding="sm"
                            unelevated
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                          >
                            <q-fab-action
                              @click="editProject(project)"
                              color="info"
                              icon="edit"
                              ><q-tooltip
                                anchor="top middle"
                                self="center middle"
                              >
                                Edit
                              </q-tooltip></q-fab-action
                            >
                            <q-fab-action
                              @click="deleteProject(project)"
                              color="negative"
                              icon="delete"
                              ><q-tooltip
                                anchor="top middle"
                                self="center middle"
                              >
                                Delete
                              </q-tooltip></q-fab-action
                            >
                          </q-fab>

                          <q-btn
                            flat
                            @click="orderProjectNext(project)"
                            round
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            icon="keyboard_arrow_down"
                          />
                        </q-card-actions>
                      </q-card-section>
                    </q-card>
                  </q-scroll-area>
                </div>

                <q-dialog v-model="dialogToggle[2]" persistent>
                  <q-card style="width: 400px">
                    <q-card-section class="row items-center">
                      <span class="text-h6">{{
                        showEditBtn ? "Edit Project" : "Add Project"
                      }}</span>
                    </q-card-section>

                    <q-separator />

                    <q-card-section>
                      <div class="q-mt-sm">
                        <q-select
                          standout
                          ref="projectOneRef"
                          v-model="tempProjectResource.groupId"
                          :options="tempListGroup"
                          :label="labelNameFocusProject[0]"
                          option-value="id"
                          option-label="name"
                          emit-value
                          map-options
                          options-selected-class="text-accent"
                          @filter="filterProject"
                          fill-input
                          input-debounce="200"
                          hide-selected
                          use-input
                          :rules="[(val) => !!val || 'Project is required']"
                          lazy-rules="ondemand"
                          hide-bottom-space
                          :label-color="colorFocusProject[0]"
                          @focus="
                            colorFocusProject[0] = 'white';
                            labelNameFocusProject[0] = 'Search by project';
                          "
                          @blur="
                            colorFocusProject[0] = '';
                            labelNameFocusProject[0] = 'Project:';
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

                      <div class="q-mt-sm">
                        <q-input
                          ref="projectTwoRef"
                          standout
                          clearable
                          maxlength="500"
                          v-model="tempProjectResource.position"
                          type="text"
                          label="Position:"
                          :label-color="colorFocusProject[1]"
                          @focus="colorFocusProject[1] = 'white'"
                          @blur="colorFocusProject[1] = ''"
                          hide-bottom-space
                          :rules="[(val) => !!val || 'Position is required']"
                          lazy-rules="ondemand"
                        >
                        </q-input>
                      </div>

                      <div class="q-mt-sm">
                        <q-input
                          ref="projectThreeRef"
                          standout
                          clearable
                          maxlength="500"
                          autogrow
                          v-model="tempProjectResource.responsibilities"
                          type="text"
                          label="Responsibilities:"
                          :label-color="colorFocusProject[2]"
                          @focus="colorFocusProject[2] = 'white'"
                          @blur="colorFocusProject[2] = ''"
                          :rules="[
                            (val) => !!val || 'Responsibilities is required',
                          ]"
                          lazy-rules="ondemand"
                          hide-bottom-space
                        >
                        </q-input>
                      </div>

                      <div class="q-mt-sm">
                        <q-input
                          ref="projectFourRef"
                          standout
                          maxlength="500"
                          v-model="tempProjectResource.startDate"
                          type="text"
                          placeholder="YYYY-MM-DD"
                          mask="####-##-##"
                          stack-label
                          label="Start Date:"
                          :label-color="colorFocusProject[3]"
                          @focus="colorFocusProject[3] = 'white'"
                          @blur="colorFocusProject[3] = ''"
                          :rules="[
                            (val) => !!val || 'Start Date is required',
                            (val) =>
                              validateDate(val) || 'Start Date is invalid',
                          ]"
                          lazy-rules="ondemand"
                          hide-bottom-space
                        >
                          <template v-slot:append>
                            <q-icon name="event" class="cursor-pointer">
                              <q-popup-proxy
                                cover
                                transition-show="scale"
                                transition-hide="scale"
                              >
                                <q-date
                                  v-model="tempProjectResource.startDate"
                                  mask="YYYY-MM-DD"
                                >
                                  <div class="row items-center justify-end">
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

                      <div class="q-mt-sm">
                        <q-input
                          ref="projectFiveRef"
                          standout
                          maxlength="500"
                          v-model="tempProjectResource.endDate"
                          type="text"
                          placeholder="YYYY-MM-DD"
                          mask="####-##-##"
                          stack-label
                          label="End Date:"
                          :label-color="colorFocusProject[4]"
                          @focus="colorFocusProject[4] = 'white'"
                          @blur="colorFocusProject[4] = ''"
                          :rules="[
                            (val) => !!val || 'End Date is required',
                            (val) => validateDate(val) || 'DoB is invalid',
                          ]"
                          lazy-rules="ondemand"
                          hide-bottom-space
                        >
                          <template v-slot:append>
                            <q-icon name="event" class="cursor-pointer">
                              <q-popup-proxy
                                cover
                                transition-show="scale"
                                transition-hide="scale"
                              >
                                <q-date
                                  v-model="tempProjectResource.endDate"
                                  mask="YYYY-MM-DD"
                                >
                                  <div class="row items-center justify-end">
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
                    </q-card-section>

                    <q-card-actions align="right">
                      <q-btn
                        flat
                        label="Cancel"
                        color="primary"
                        v-close-popup
                      />
                      <q-btn
                        flat
                        label="Add"
                        color="info"
                        @click="addProject"
                        v-show="!showEditBtn"
                      />
                      <q-btn
                        flat
                        label="Save"
                        color="info"
                        @click="saveProject"
                        v-show="showEditBtn"
                      />
                    </q-card-actions>
                  </q-card>
                </q-dialog>

                <q-dialog v-model="deleteToggle[2]" persistent>
                  <q-card>
                    <q-card-section class="row items-center">
                      <span class="text-h6"
                        >Delete {{ tempDeleteProject.name }} project?</span
                      >
                    </q-card-section>

                    <q-separator />

                    <q-card-section>
                      <span>This can’t be undone and it will be removed.</span>
                    </q-card-section>

                    <q-card-actions align="right">
                      <q-btn
                        flat
                        v-close-popup
                        label="Cancel"
                        color="primary"
                      />
                      <q-btn
                        flat
                        label="Delete"
                        color="negative"
                        @click="saveDeleteProject"
                      />
                    </q-card-actions>
                  </q-card>
                </q-dialog>
              </q-tab-panel>

              <q-tab-panel name="3" class="tab-project flex flex-center">
                <div class="full-width relative-position flex flex-center">
                  <q-btn-group rounded flat unelevated>
                    <q-btn
                      color="primary"
                      label="<"
                      @click="orderTabPrev()"
                      size="md"
                    />
                    <q-btn
                      :color="booleanTab(3) ? 'primary' : 'grey'"
                      label="Work History"
                      @click="toggleTab(3)"
                      size="md"
                    >
                      <q-tooltip anchor="top middle" self="center middle">
                        {{ booleanTab(3) ? "Hide" : "Show" }}
                      </q-tooltip></q-btn
                    >
                    <q-btn
                      color="primary"
                      label=">"
                      @click="orderTabNext()"
                      size="md"
                    />
                  </q-btn-group>

                  <q-btn
                    size="12px"
                    class="absolute-top-right"
                    icon="add"
                    color="primary"
                    round
                    unelevated
                    @click="openDialog(3)"
                  />
                </div>

                <div class="fit">
                  <q-scroll-area class="q-mt-md" style="height: 411px">
                    <q-card
                      v-for="(workHistory, index) in listTempWorkHistory"
                      :key="index"
                      bordered
                      class="inside"
                    >
                      <q-card-section horizontal class="row">
                        <q-card-section class="col-10">
                          <div class="q-mb-sm">
                            <q-badge
                              class="text-subtitle2"
                              :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            >
                              <span>{{ workHistory.companyName }}:</span>
                            </q-badge>
                          </div>

                          <div>
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                              >Position:</q-badge
                            >
                            {{ workHistory.position }}
                          </div>

                          <div>
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                              >Start Date:</q-badge
                            >
                            {{ convertDateTimeToDate(workHistory.startDate) }}
                          </div>

                          <div v-show="workHistory.endDate">
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                              >End Date:</q-badge
                            >
                            {{ convertDateTimeToDate(workHistory.endDate) }}
                          </div>
                        </q-card-section>

                        <q-card-actions vertical class="col-2 justify-around">
                          <q-btn
                            @click="orderWorkHistoryPrev(workHistory)"
                            flat
                            round
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            icon="keyboard_arrow_up"
                          />

                          <q-fab
                            class="q-my-sm"
                            icon="more_horiz"
                            direction="left"
                            padding="sm"
                            unelevated
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                          >
                            <q-fab-action
                              @click="editWorkHistory(workHistory)"
                              color="info"
                              icon="edit"
                              ><q-tooltip
                                anchor="top middle"
                                self="center middle"
                              >
                                Edit
                              </q-tooltip></q-fab-action
                            >
                            <q-fab-action
                              @click="deleteWorkHistory(workHistory)"
                              color="negative"
                              icon="delete"
                              ><q-tooltip
                                anchor="top middle"
                                self="center middle"
                              >
                                Delete
                              </q-tooltip></q-fab-action
                            >
                          </q-fab>

                          <q-btn
                            flat
                            @click="orderWorkHistoryNext(workHistory)"
                            round
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            icon="keyboard_arrow_down"
                          />
                        </q-card-actions>
                      </q-card-section>
                    </q-card>
                  </q-scroll-area>
                </div>

                <q-dialog v-model="dialogToggle[3]" persistent>
                  <q-card style="width: 400px">
                    <q-card-section class="row items-center">
                      <span class="text-h6">{{
                        showEditBtn ? "Edit Work History" : "Add History"
                      }}</span>
                    </q-card-section>

                    <q-separator />

                    <q-card-section>
                      <div class="q-mt-sm">
                        <q-input
                          ref="workHistoryOneRef"
                          standout
                          clearable
                          maxlength="250"
                          v-model="tempWorkHistoryResource.companyName"
                          type="text"
                          label="Company Name:"
                          :label-color="colorFocusWorkHistory[0]"
                          @focus="colorFocusWorkHistory[0] = 'white'"
                          @blur="colorFocusWorkHistory[0] = ''"
                          :rules="[
                            (val) => !!val || 'Company Name is required',
                          ]"
                          lazy-rules="ondemand"
                          hide-bottom-space
                        >
                        </q-input>
                      </div>

                      <div class="q-mt-sm">
                        <q-input
                          ref="workHistoryTwoRef"
                          standout
                          clearable
                          maxlength="250"
                          v-model="tempWorkHistoryResource.position"
                          type="text"
                          label="Position:"
                          :label-color="colorFocusWorkHistory[1]"
                          @focus="colorFocusWorkHistory[1] = 'white'"
                          @blur="colorFocusWorkHistory[1] = ''"
                          hide-bottom-space
                          :rules="[(val) => !!val || 'Position is required']"
                          lazy-rules="ondemand"
                        >
                        </q-input>
                      </div>

                      <div class="q-mt-sm">
                        <q-input
                          ref="workHistoryThreeRef"
                          standout
                          maxlength="250"
                          v-model="tempWorkHistoryResource.startDate"
                          type="text"
                          placeholder="YYYY-MM-DD"
                          mask="####-##-##"
                          stack-label
                          label="Start Date:"
                          :label-color="colorFocusWorkHistory[2]"
                          @focus="colorFocusWorkHistory[2] = 'white'"
                          @blur="colorFocusWorkHistory[2] = ''"
                          :rules="[
                            (val) => !!val || 'Start Date is required',
                            (val) => validateDate(val) || 'DoB is invalid',
                          ]"
                          lazy-rules="ondemand"
                          hide-bottom-space
                        >
                          <template v-slot:append>
                            <q-icon name="event" class="cursor-pointer">
                              <q-popup-proxy
                                cover
                                transition-show="scale"
                                transition-hide="scale"
                              >
                                <q-date
                                  v-model="tempWorkHistoryResource.startDate"
                                  mask="YYYY-MM-DD"
                                >
                                  <div class="row items-center justify-end">
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

                      <div class="q-mt-sm">
                        <q-input
                          ref="workHistoryFourRef"
                          standout
                          maxlength="250"
                          v-model="tempWorkHistoryResource.endDate"
                          type="text"
                          placeholder="YYYY-MM-DD"
                          mask="####-##-##"
                          stack-label
                          label="End Date:"
                          :label-color="colorFocusWorkHistory[3]"
                          @focus="colorFocusWorkHistory[3] = 'white'"
                          @blur="colorFocusWorkHistory[3] = ''"
                          hide-bottom-space
                          :rules="[
                            (val) => validateDate(val) || 'DoB is invalid',
                          ]"
                          lazy-rules="ondemand"
                        >
                          <template v-slot:append>
                            <q-icon name="event" class="cursor-pointer">
                              <q-popup-proxy
                                cover
                                transition-show="scale"
                                transition-hide="scale"
                              >
                                <q-date
                                  v-model="tempWorkHistoryResource.endDate"
                                  mask="YYYY-MM-DD"
                                >
                                  <div class="row items-center justify-end">
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
                    </q-card-section>

                    <q-card-actions align="right">
                      <q-btn
                        flat
                        label="Cancel"
                        color="primary"
                        v-close-popup
                      />
                      <q-btn
                        flat
                        label="Add"
                        color="info"
                        @click="addWorkHistory"
                        v-show="!showEditBtn"
                      />
                      <q-btn
                        flat
                        label="Save"
                        color="info"
                        @click="saveWorkHistory"
                        v-show="showEditBtn"
                      />
                    </q-card-actions>
                  </q-card>
                </q-dialog>

                <q-dialog v-model="deleteToggle[3]" persistent>
                  <q-card>
                    <q-card-section class="row items-center">
                      <span class="text-h6"
                        >Delete {{ tempDeleteWorkHistory.companyName }} work
                        history?</span
                      >
                    </q-card-section>

                    <q-separator />

                    <q-card-section>
                      <span>This can’t be undone and it will be removed.</span>
                    </q-card-section>

                    <q-card-actions align="right">
                      <q-btn
                        flat
                        v-close-popup
                        label="Cancel"
                        color="primary"
                      />
                      <q-btn
                        flat
                        label="Delete"
                        color="negative"
                        @click="saveDeleteWorkHistory"
                      />
                    </q-card-actions>
                  </q-card>
                </q-dialog>
              </q-tab-panel>

              <q-tab-panel name="4" class="tab-project flex flex-center">
                <div class="full-width relative-position flex flex-center">
                  <q-btn-group rounded flat unelevated>
                    <q-btn
                      color="primary"
                      label="<"
                      @click="orderTabPrev()"
                      size="md"
                    />
                    <q-btn
                      :color="booleanTab(4) ? 'primary' : 'grey'"
                      label="Education"
                      @click="toggleTab(4)"
                      size="md"
                    >
                      <q-tooltip anchor="top middle" self="center middle">
                        {{ booleanTab(4) ? "Hide" : "Show" }}
                      </q-tooltip></q-btn
                    >
                    <q-btn
                      color="primary"
                      label=">"
                      @click="orderTabNext()"
                      size="md"
                    />
                  </q-btn-group>

                  <q-btn
                    size="12px"
                    class="absolute-top-right"
                    icon="add"
                    color="primary"
                    round
                    unelevated
                    @click="openDialog(4)"
                  />
                </div>

                <div class="fit">
                  <q-scroll-area class="q-mt-md" style="height: 411px">
                    <q-card
                      v-for="(education, index) in listTempEducation"
                      :key="index"
                      bordered
                      class="inside"
                    >
                      <q-card-section horizontal class="row">
                        <q-card-section class="col-10">
                          <div class="q-mb-sm">
                            <q-badge
                              class="text-subtitle2"
                              :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            >
                              <span>{{ education.collegeName }}:</span>
                            </q-badge>
                          </div>

                          <div>
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                              >Position:</q-badge
                            >
                            {{ education.major }}
                          </div>

                          <div>
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                              >Start Date:</q-badge
                            >
                            {{ convertDateTimeToDate(education.startDate) }}
                          </div>

                          <div v-show="education.endDate">
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                              >End Date:</q-badge
                            >
                            {{ convertDateTimeToDate(education.endDate) }}
                          </div>
                        </q-card-section>

                        <q-card-actions vertical class="col-2 justify-around">
                          <q-btn
                            @click="orderEducationPrev(education)"
                            flat
                            round
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            icon="keyboard_arrow_up"
                          />

                          <q-fab
                            class="q-my-sm"
                            icon="more_horiz"
                            direction="left"
                            padding="sm"
                            unelevated
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                          >
                            <q-fab-action
                              @click="editEducation(education)"
                              color="info"
                              icon="edit"
                              ><q-tooltip
                                anchor="top middle"
                                self="center middle"
                              >
                                Edit
                              </q-tooltip></q-fab-action
                            >
                            <q-fab-action
                              @click="deleteEducation(education)"
                              color="negative"
                              icon="delete"
                              ><q-tooltip
                                anchor="top middle"
                                self="center middle"
                              >
                                Delete
                              </q-tooltip></q-fab-action
                            >
                          </q-fab>

                          <q-btn
                            flat
                            @click="orderEducationNext(education)"
                            round
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            icon="keyboard_arrow_down"
                          />
                        </q-card-actions>
                      </q-card-section>
                    </q-card>
                  </q-scroll-area>
                </div>

                <q-dialog v-model="dialogToggle[4]" persistent>
                  <q-card style="width: 400px">
                    <q-card-section class="row items-center">
                      <span class="text-h6">{{
                        showEditBtn ? "Edit Education" : "Add Education"
                      }}</span>
                    </q-card-section>

                    <q-separator />

                    <q-card-section>
                      <div class="q-mt-sm">
                        <q-input
                          ref="educationOneRef"
                          standout
                          clearable
                          maxlength="250"
                          v-model="tempEducationResource.collegeName"
                          type="text"
                          label="College Name:"
                          :label-color="colorFocusEducation[0]"
                          @focus="colorFocusEducation[0] = 'white'"
                          @blur="colorFocusEducation[0] = ''"
                          :rules="[
                            (val) => !!val || 'College Name is required',
                          ]"
                          lazy-rules="ondemand"
                          hide-bottom-space
                        >
                        </q-input>
                      </div>

                      <div class="q-mt-sm">
                        <q-input
                          ref="educationTwoRef"
                          standout
                          clearable
                          maxlength="250"
                          v-model="tempEducationResource.major"
                          type="text"
                          label="Major:"
                          :label-color="colorFocusEducation[1]"
                          @focus="colorFocusEducation[1] = 'white'"
                          @blur="colorFocusEducation[1] = ''"
                          hide-bottom-space
                          :rules="[(val) => !!val || 'Major is required']"
                          lazy-rules="ondemand"
                        >
                        </q-input>
                      </div>

                      <div class="q-mt-sm">
                        <q-input
                          ref="educationThreeRef"
                          standout
                          maxlength="250"
                          v-model="tempEducationResource.startDate"
                          type="text"
                          placeholder="YYYY-MM-DD"
                          mask="####-##-##"
                          stack-label
                          label="Start Date:"
                          :label-color="colorFocusEducation[2]"
                          @focus="colorFocusEducation[2] = 'white'"
                          @blur="colorFocusEducation[2] = ''"
                          :rules="[
                            (val) => !!val || 'Start Date is required',
                            (val) => validateDate(val) || 'DoB is invalid',
                          ]"
                          lazy-rules="ondemand"
                          hide-bottom-space
                        >
                          <template v-slot:append>
                            <q-icon name="event" class="cursor-pointer">
                              <q-popup-proxy
                                cover
                                transition-show="scale"
                                transition-hide="scale"
                              >
                                <q-date
                                  v-model="tempEducationResource.startDate"
                                  mask="YYYY-MM-DD"
                                >
                                  <div class="row items-center justify-end">
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

                      <div class="q-mt-sm">
                        <q-input
                          ref="workHistoryFourRef"
                          standout
                          maxlength="250"
                          v-model="tempEducationResource.endDate"
                          type="text"
                          placeholder="YYYY-MM-DD"
                          mask="####-##-##"
                          stack-label
                          label="End Date:"
                          :label-color="colorFocusEducation[3]"
                          @focus="colorFocusEducation[3] = 'white'"
                          @blur="colorFocusEducation[3] = ''"
                          hide-bottom-space
                          :rules="[
                            (val) => validateDate(val) || 'DoB is invalid',
                          ]"
                        >
                          <template v-slot:append>
                            <q-icon name="event" class="cursor-pointer">
                              <q-popup-proxy
                                cover
                                transition-show="scale"
                                transition-hide="scale"
                              >
                                <q-date
                                  v-model="tempEducationResource.endDate"
                                  mask="YYYY-MM-DD"
                                >
                                  <div class="row items-center justify-end">
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
                    </q-card-section>

                    <q-card-actions align="right">
                      <q-btn
                        flat
                        label="Cancel"
                        color="primary"
                        v-close-popup
                      />
                      <q-btn
                        flat
                        label="Add"
                        color="info"
                        @click="addEducation"
                        v-show="!showEditBtn"
                      />
                      <q-btn
                        flat
                        label="Save"
                        color="info"
                        @click="saveEducation"
                        v-show="showEditBtn"
                      />
                    </q-card-actions>
                  </q-card>
                </q-dialog>

                <q-dialog v-model="deleteToggle[4]" persistent>
                  <q-card>
                    <q-card-section class="row items-center">
                      <span class="text-h6"
                        >Delete
                        {{ tempDeleteEducation.collegeName }} education?</span
                      >
                    </q-card-section>

                    <q-separator />

                    <q-card-section>
                      <span>This can’t be undone and it will be removed.</span>
                    </q-card-section>

                    <q-card-actions align="right">
                      <q-btn
                        flat
                        v-close-popup
                        label="Cancel"
                        color="primary"
                      />
                      <q-btn
                        flat
                        label="Delete"
                        color="negative"
                        @click="saveDeleteEducation"
                      />
                    </q-card-actions>
                  </q-card>
                </q-dialog>
              </q-tab-panel>

              <q-tab-panel name="5" class="tab-project flex flex-center">
                <div class="full-width relative-position flex flex-center">
                  <q-btn-group rounded flat unelevated>
                    <q-btn
                      color="primary"
                      label="<"
                      @click="orderTabPrev()"
                      size="md"
                    />
                    <q-btn
                      :color="booleanTab(5) ? 'primary' : 'grey'"
                      label="Certificate"
                      @click="toggleTab(5)"
                      size="md"
                    >
                      <q-tooltip anchor="top middle" self="center middle">
                        {{ booleanTab(5) ? "Hide" : "Show" }}
                      </q-tooltip></q-btn
                    >
                    <q-btn
                      color="primary"
                      label=">"
                      @click="orderTabNext()"
                      size="md"
                    />
                  </q-btn-group>

                  <q-btn
                    size="12px"
                    class="absolute-top-right"
                    icon="add"
                    color="primary"
                    round
                    unelevated
                    @click="openDialog(5)"
                  />
                </div>

                <div class="fit">
                  <q-scroll-area class="q-mt-md" style="height: 411px">
                    <q-card
                      v-for="(certificate, index) in listTempCertificate"
                      :key="index"
                      bordered
                      class="inside"
                    >
                      <q-card-section horizontal class="row">
                        <q-card-section class="col-10">
                          <div class="q-mb-sm">
                            <q-badge
                              class="text-subtitle2"
                              :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            >
                              <span>{{ certificate.name }}:</span>
                            </q-badge>
                          </div>

                          <div>
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                              >Position:</q-badge
                            >
                            {{ certificate.provider }}
                          </div>

                          <div>
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                              >Start Date:</q-badge
                            >
                            {{ convertDateTimeToDate(certificate.startDate) }}
                          </div>

                          <div v-show="certificate.endDate">
                            <q-badge
                              :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                              style="font-size: 14px"
                              >End Date:</q-badge
                            >
                            {{ convertDateTimeToDate(certificate.endDate) }}
                          </div>
                        </q-card-section>

                        <q-card-actions vertical class="col-2 justify-around">
                          <q-btn
                            @click="orderCertificatePrev(certificate)"
                            flat
                            round
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            icon="keyboard_arrow_up"
                          />

                          <q-fab
                            class="q-my-sm"
                            icon="more_horiz"
                            direction="left"
                            padding="sm"
                            unelevated
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                          >
                            <q-fab-action
                              @click="editCertificate(certificate)"
                              color="info"
                              icon="edit"
                              ><q-tooltip
                                anchor="top middle"
                                self="center middle"
                              >
                                Edit
                              </q-tooltip></q-fab-action
                            >
                            <q-fab-action
                              @click="deleteCertificate(certificate)"
                              color="negative"
                              icon="delete"
                              ><q-tooltip
                                anchor="top middle"
                                self="center middle"
                              >
                                Delete
                              </q-tooltip></q-fab-action
                            >
                          </q-fab>

                          <q-btn
                            flat
                            @click="orderCertificateNext(certificate)"
                            round
                            :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            icon="keyboard_arrow_down"
                          />
                        </q-card-actions>
                      </q-card-section>
                    </q-card>
                  </q-scroll-area>
                </div>

                <q-dialog v-model="dialogToggle[5]" persistent>
                  <q-card style="width: 400px">
                    <q-card-section class="row items-center">
                      <span class="text-h6">{{
                        showEditBtn ? "Edit Certificate" : "Add Certificate"
                      }}</span>
                    </q-card-section>

                    <q-separator />

                    <q-card-section>
                      <div class="q-mt-sm">
                        <q-input
                          ref="certificateOneRef"
                          standout
                          clearable
                          maxlength="250"
                          v-model="tempCertificateResource.name"
                          type="text"
                          label="Name:"
                          :label-color="colorFocusCertificate[0]"
                          @focus="colorFocusCertificate[0] = 'white'"
                          @blur="colorFocusCertificate[0] = ''"
                          :rules="[(val) => !!val || 'Name is required']"
                          hide-bottom-space
                        >
                        </q-input>
                      </div>

                      <div class="q-mt-sm">
                        <q-input
                          ref="certificateTwoRef"
                          standout
                          clearable
                          maxlength="250"
                          v-model="tempCertificateResource.provider"
                          type="text"
                          label="Provider:"
                          :label-color="colorFocusCertificate[1]"
                          @focus="colorFocusCertificate[1] = 'white'"
                          @blur="colorFocusCertificate[1] = ''"
                          hide-bottom-space
                          :rules="[(val) => !!val || 'Provider is required']"
                        >
                        </q-input>
                      </div>

                      <div class="q-mt-sm">
                        <q-input
                          ref="certificateThreeRef"
                          standout
                          maxlength="250"
                          v-model="tempCertificateResource.startDate"
                          type="text"
                          placeholder="YYYY-MM-DD"
                          mask="####-##-##"
                          stack-label
                          label="Start Date:"
                          :label-color="colorFocusCertificate[2]"
                          @focus="colorFocusCertificate[2] = 'white'"
                          @blur="colorFocusCertificate[2] = ''"
                          :rules="[
                            (val) => !!val || 'Start Date is required',
                            (val) => validateDate(val) || 'DoB is invalid',
                          ]"
                          hide-bottom-space
                        >
                          <template v-slot:append>
                            <q-icon name="event" class="cursor-pointer">
                              <q-popup-proxy
                                cover
                                transition-show="scale"
                                transition-hide="scale"
                              >
                                <q-date
                                  v-model="tempCertificateResource.startDate"
                                  mask="YYYY-MM-DD"
                                >
                                  <div class="row items-center justify-end">
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

                      <div class="q-mt-sm">
                        <q-input
                          ref="workHistoryFourRef"
                          standout
                          maxlength="250"
                          v-model="tempCertificateResource.endDate"
                          type="text"
                          placeholder="YYYY-MM-DD"
                          mask="####-##-##"
                          stack-label
                          label="End Date:"
                          :label-color="colorFocusCertificate[3]"
                          @focus="colorFocusCertificate[3] = 'white'"
                          @blur="colorFocusCertificate[3] = ''"
                          hide-bottom-space
                          :rules="[
                            (val) => validateDate(val) || 'DoB is invalid',
                          ]"
                        >
                          <template v-slot:append>
                            <q-icon name="event" class="cursor-pointer">
                              <q-popup-proxy
                                cover
                                transition-show="scale"
                                transition-hide="scale"
                              >
                                <q-date
                                  v-model="tempCertificateResource.endDate"
                                  mask="YYYY-MM-DD"
                                >
                                  <div class="row items-center justify-end">
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
                    </q-card-section>

                    <q-card-actions align="right">
                      <q-btn
                        flat
                        label="Cancel"
                        color="primary"
                        v-close-popup
                      />
                      <q-btn
                        flat
                        label="Add"
                        color="info"
                        @click="addCertificate"
                        v-show="!showEditBtn"
                      />
                      <q-btn
                        flat
                        label="Save"
                        color="info"
                        @click="saveCertificate"
                        v-show="showEditBtn"
                      />
                    </q-card-actions>
                  </q-card>
                </q-dialog>

                <q-dialog v-model="deleteToggle[5]" persistent>
                  <q-card>
                    <q-card-section class="row items-center">
                      <span class="text-h6"
                        >Delete
                        {{ tempDeleteCertificate.name }} certificate?</span
                      >
                    </q-card-section>

                    <q-separator />

                    <q-card-section>
                      <span>This can’t be undone and it will be removed.</span>
                    </q-card-section>

                    <q-card-actions align="right">
                      <q-btn
                        flat
                        v-close-popup
                        label="Cancel"
                        color="primary"
                      />
                      <q-btn
                        flat
                        label="Delete"
                        color="negative"
                        @click="saveDeleteCertificate"
                      />
                    </q-card-actions>
                  </q-card>
                </q-dialog>
              </q-tab-panel>
            </q-tab-panels>
          </q-card>
        </div>
      </div>
    </div>
  </q-page>
</template>

<script>
import { defineComponent } from "vue";
import { mapGetters, mapActions } from "vuex";
import { useQuasar, date, extend } from "quasar";
import { api } from "src/boot/axios";

export default defineComponent({
  name: "Edit Employee",

  props: {
    employeeTransfer: Object,
    statusUpdateTransfer: Boolean,
  },

  data() {
    return {
      tab: "1",
      tabModel: [
        { id: "1", name: "Skill" },
        { id: "2", name: "Project" },
        { id: "3", name: "Work History" },
        { id: "4", name: "Education" },
        { id: "5", name: "Certificate" },
      ],

      avatarDefault: "",
      imageURL: null,
      imageFile: null,

      employeeInfor: {
        firstName: "",
        lastName: "",
        email: null,
        description: "",
        phone: null,
        dateOfBirth: "",
        positionId: null,
        groupId: null,
        gender: "",
        orderIndex: [],
      },

      tempCategoryPersonResource: {
        id: 0,
        personId: 0,
        categoryId: 0,
        technologies: [],
      },
      listTempCategory: [],

      tempProjectResource: {
        id: 0,
        position: "",
        responsibilities: "",
        startDate: "",
        endDate: null,
        personId: 0,
        groupId: 0,
      },
      listTempProject: [],

      tempWorkHistoryResource: {
        id: 0,
        companyName: "",
        position: "",
        startDate: null,
        endDate: null,
        personId: 0,
      },
      listTempWorkHistory: [],

      tempEducationResource: {
        id: 0,
        collegeName: "",
        major: "",
        startDate: "",
        endDate: null,
        personId: 0,
      },
      listTempEducation: [],

      tempCertificateResource: {
        id: 0,
        name: "",
        provider: "",
        startDate: "",
        endDate: null,
        personId: 0,
      },
      listTempCertificate: [],

      tempDeleteSkill: null,
      tempDeleteProject: null,
      tempDeleteWorkHistory: null,
      tempDeleteEducation: null,
      tempDeleteCertificate: null,

      loadingSave: false,

      labelColorFocus: [],
      colorFocusSkill: [],
      colorFocusProject: [],
      colorFocusWorkHistory: [],
      colorFocusEducation: [],
      colorFocusCertificate: [],
      labelNameFocusSkill: ["Category:", "Skill:"],
      labelNameFocusProject: ["Project:"],
      showEditBtn: false,

      listGender: [
        {
          name: "Male",
          id: 1,
        },
        {
          name: "Female",
          id: 2,
        },
        {
          name: "Sexless",
          id: 3,
        },
      ],
      dialogToggle: [],
      deleteToggle: [],
      tempOrderIndex: [],
      tempListSkillCategory: [],
      tempListCategory: [],
      listCategory: [],
      tempListPosition: [],
      listPosition: [],
      tempListGroup: [],
      tempListWork: [],
      listGroup: [],
    };
  },
  methods: {
    ...mapActions("auth", ["validateToken"]),

    async findCategory(keyword, seedValue) {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      // Request API
      let result = await api
        .get(
          `/api/v1/category/search?filterName=${!keyword ? "" : keyword.trim()}`
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
        this.tempListCategory = result.resource;
        if (seedValue) this.listCategory = result.resource;
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async findGroup(keyword, seedValue, component) {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      // Request API
      let result = await api
        .get(
          `/api/v1/group/search?filterName=${!keyword ? "" : keyword.trim()}`
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
        if (component == "project") this.tempListGroup = result.resource;
        if (component == "work") this.tempListWork = result.resource;
        if (seedValue) this.listGroup = result.resource;
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async getByCategoryId(id) {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      // Request API
      let result = await api
        .get(`/api/v1/category/${id}`)
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
        let category = result.resource;
        this.tempListCategory = [];
        this.tempListCategory.unshift(category);
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async getByGroupId(id) {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      // Request API
      let result = await api
        .get(`/api/v1/group/${id}`)
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
        return result.resource;
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
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

        // Mode edit
        if (this.employeeInfor.positionId) {
          this.tempListPosition = this.listPosition.filter(
            (v) => v.id == this.employeeInfor.positionId
          );
        }
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async getAvatarUrl() {
      // Request API
      let result = await api
        .get(`/api/v1/image/default-image`)
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
        this.avatarDefault = result?.resource?.originalImagePath;
      }
    },
    previewImage() {
      this.imageURL = this.imageFile
        ? URL.createObjectURL(this.imageFile)
        : null;
    },
    clearTempImage() {
      this.imageURL = this.avatarDefault;
    },
    onRejected(rejectedEntries) {
      this.$q.notify({
        type: "negative",
        message: `Image size must lower than 5 MB`,
      });
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
    filterCategory(val, update, abort) {
      update(async () => {
        if (val.length > 0 && val.length < 2)
          this.tempListCategory = this.listCategory;
        if (val.length >= 2) await this.findCategory(val, false);
      });
    },
    filterProject(val, update, abort) {
      update(async () => {
        if (!this.tempListGroup?.length) this.tempListGroup = this.listGroup;
        if (val.length > 0 && val.length < 2)
          this.tempListGroup = this.listGroup;
        if (val.length >= 2) await this.findGroup(val, false, "project");
      });
    },
    filterWork(val, update, abort) {
      update(async () => {
        if (!val || val.length < 2) this.tempListWork = this.listGroup;
        if (val.length >= 2) await this.findGroup(val, false, "work");
      });
    },
    filterSkillBelongWithCategory(val, update, abort) {
      update(() => {
        let temp = this.getSkillBelongWithCategory();
        if (!val) {
          this.tempListSkillCategory = temp;
        } else {
          let needle = val.toLowerCase();
          this.tempListSkillCategory = temp.filter(
            (v) => v.name.toLowerCase().indexOf(needle) > -1
          );
        }
      });
    },
    getSkillBelongWithCategory() {
      let categoryId = this.tempCategoryPersonResource.categoryId;

      let obj = this.tempListCategory.find((x) => x.id == categoryId);

      return obj.technologies;
    },
    openDialog(index) {
      if (index == 1) {
        this.tempCategoryPersonResource.categoryId = 0;
        this.tempCategoryPersonResource.technologies = [];
      }

      if (index == 2) {
        this.tempProjectResource.position = "";
        this.tempProjectResource.responsibilities = "";
        this.tempProjectResource.startDate = "";
        this.tempProjectResource.endDate = null;
        this.tempProjectResource.groupId = 0;
      }

      if (index == 3) {
        this.tempWorkHistoryResource.companyName = "";
        this.tempWorkHistoryResource.position = "";
        this.tempWorkHistoryResource.startDate = null;
        this.tempWorkHistoryResource.endDate = null;
      }

      if (index == 4) {
        this.tempEducationResource.collegeName = "";
        this.tempEducationResource.major = "";
        this.tempEducationResource.startDate = null;
        this.tempEducationResource.endDate = null;
      }

      if (index == 5) {
        this.tempCertificateResource.name = "";
        this.tempCertificateResource.provider = "";
        this.tempCertificateResource.startDate = null;
        this.tempCertificateResource.endDate = null;
      }

      this.dialogToggle[index] = true;
    },
    orderTabNext() {
      // Print UI
      let countTab = this.tabModel.length;
      let indexTab = this.tabModel.findIndex((x) => x.id == this.tab);

      if (indexTab == countTab - 1) {
        let temp = this.tabModel.slice(0, indexTab);
        temp.unshift(this.tabModel[indexTab]);
        this.tabModel = temp;
      } else {
        for (let i = 0; i < countTab; i++) {
          if (i == indexTab) {
            let temp = this.tabModel[i];
            this.tabModel[i] = this.tabModel[i + 1];
            this.tabModel[i + 1] = temp;
            break;
          }
        }
      }

      // Array send to server
      let countModel = this.employeeInfor.orderIndex.length;
      let indexModel = this.employeeInfor.orderIndex.findIndex(
        (x) => x == parseInt(this.tab)
      );

      if (indexModel == countModel - 1) {
        let temp = this.employeeInfor.orderIndex.slice(0, indexModel);
        temp.unshift(this.employeeInfor.orderIndex[indexModel]);
        this.employeeInfor.orderIndex = temp;
      } else {
        for (let i = 0; i < countModel; i++) {
          if (i == indexModel) {
            let temp = this.employeeInfor.orderIndex[i];
            this.employeeInfor.orderIndex[i] =
              this.employeeInfor.orderIndex[i + 1];
            this.employeeInfor.orderIndex[i + 1] = temp;
            break;
          }
        }
      }
    },
    orderTabPrev() {
      // Array send to server
      let countTab = this.tabModel.length;
      let indexTab = this.tabModel.findIndex((x) => x.id == this.tab);

      if (indexTab == 0) {
        let temp = this.tabModel.slice(1);
        temp.push(this.tabModel[indexTab]);
        this.tabModel = temp;
      } else {
        for (let i = 0; i < countTab; i++) {
          if (i == indexTab) {
            let temp = this.tabModel[i];
            this.tabModel[i] = this.tabModel[i - 1];
            this.tabModel[i - 1] = temp;
            break;
          }
        }
      }

      // Array send to server
      let countModel = this.employeeInfor.orderIndex.length;
      let indexModel = this.employeeInfor.orderIndex.findIndex(
        (x) => x == parseInt(this.tab)
      );

      if (indexModel == 0) {
        let temp = this.employeeInfor.orderIndex.slice(1);
        temp.push(this.employeeInfor.orderIndex[indexModel]);
        this.employeeInfor.orderIndex = temp;
      } else {
        for (let i = 0; i < countModel; i++) {
          if (i == indexModel) {
            let temp = this.employeeInfor.orderIndex[i];
            this.employeeInfor.orderIndex[i] =
              this.employeeInfor.orderIndex[i - 1];
            this.employeeInfor.orderIndex[i - 1] = temp;
            break;
          }
        }
      }
    },
    toggleTab(val) {
      let index = this.employeeInfor.orderIndex.findIndex(
        (x) => x == parseInt(val)
      );

      let oldIndex = this.tempOrderIndex[parseInt(val)];

      if (index >= 0) {
        this.employeeInfor.orderIndex.splice(index, 1);
        this.tempOrderIndex[parseInt(val)] = index;
      } else if (oldIndex >= 0) {
        let left = this.employeeInfor.orderIndex.slice(0, oldIndex);
        let right = this.employeeInfor.orderIndex.slice(oldIndex);

        this.employeeInfor.orderIndex = left.concat(parseInt(val), right);
      } else {
        this.employeeInfor.orderIndex.push(parseInt(val));
      }
    },
    booleanTab(val) {
      return this.employeeInfor.orderIndex.some((x) => x == parseInt(val));
    },
    async addSkill() {
      if (
        !this.$refs.categoryRef.validate() ||
        !this.$refs.skillRef.validate()
      ) {
        return null;
      }

      // Request API
      let result = await api
        .post(`/api/v1/category-person`, this.tempCategoryPersonResource)
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
        this.listTempCategory.unshift(result.resource);
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }

      this.dialogToggle[1] = false;
    },
    async editSkill(item) {
      await this.getByCategoryId(item.categoryId);

      let temp = [];

      item.technologies.forEach((x) => temp.push(x.id));

      this.tempCategoryPersonResource.categoryId = item.categoryId;
      this.tempCategoryPersonResource.id = item.id;
      this.tempCategoryPersonResource.technologies = temp;

      this.tempListSkillCategory = this.getSkillBelongWithCategory();

      this.showEditBtn = true;
      this.dialogToggle[1] = true;
    },
    async saveSkill() {
      if (
        !this.$refs.categoryRef.validate() ||
        !this.$refs.skillRef.validate()
      ) {
        return null;
      }

      // Request API
      let result = await api
        .put(
          `/api/v1/category-person/${this.tempCategoryPersonResource.id}`,
          this.tempCategoryPersonResource
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
        let index = this.listTempCategory.findIndex(
          (x) => x.id == this.tempCategoryPersonResource.id
        );

        this.listTempCategory[index] = result.resource;
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }

      this.showEditBtn = false;
      this.dialogToggle[1] = false;
    },
    deleteSkill(item) {
      this.tempDeleteSkill = item;
      this.deleteToggle[1] = true;
    },
    async saveDeleteSkill() {
      // Request API
      let result = await api
        .delete(`/api/v1/category-person/${this.tempDeleteSkill.id}`)
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
        let index = this.listTempCategory.findIndex(
          (x) => x.id == this.tempDeleteSkill.id
        );

        this.listTempCategory.splice(index, 1);
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }

      this.deleteToggle[1] = false;
    },
    async orderSkillNext(item) {
      let count = this.listTempCategory.length;
      // Set list API
      let index = this.listTempCategory.findIndex((x) => x.id == item.id);

      if (index == count - 1) {
        let temp = this.listTempCategory.slice(0, index);
        temp.unshift(this.listTempCategory[index]);
        this.listTempCategory = temp;
      } else {
        for (let i = 0; i < count; i++) {
          if (i == index) {
            let temp = this.listTempCategory[i];
            this.listTempCategory[i] = this.listTempCategory[i + 1];
            this.listTempCategory[i + 1] = temp;

            break;
          }
        }
      }

      let payload = [];
      this.listTempCategory.forEach((x) => payload.push(x.id));

      // Request API
      let result = await api
        .put(`/api/v1/category-person`, payload)
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

      if (!result.success) {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async orderSkillPrev(item) {
      let count = this.listTempCategory.length;
      // Set list API
      let index = this.listTempCategory.findIndex((x) => x.id == item.id);

      if (index == 0) {
        let temp = this.listTempCategory.slice(1);
        temp.push(this.listTempCategory[index]);
        this.listTempCategory = temp;
      } else {
        for (let i = 0; i < count; i++) {
          if (i == index) {
            let temp = this.listTempCategory[i];
            this.listTempCategory[i] = this.listTempCategory[i - 1];
            this.listTempCategory[i - 1] = temp;

            break;
          }
        }
      }

      let payload = [];
      this.listTempCategory.forEach((x) => payload.push(x.id));

      // Request API
      let result = await api
        .put(`/api/v1/category-person`, payload)
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

      if (!result.success) {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async addProject() {
      if (
        !this.$refs.projectOneRef.validate() ||
        !this.$refs.projectTwoRef.validate() ||
        !this.$refs.projectThreeRef.validate() ||
        !this.$refs.projectFourRef.validate()
      ) {
        return null;
      }

      // Request API
      let result = await api
        .post(`/api/v1/project`, this.tempProjectResource)
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
        this.listTempProject.unshift(result.resource);
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }

      this.dialogToggle[2] = false;
    },
    async saveProject() {
      if (
        !this.$refs.projectOneRef.validate() ||
        !this.$refs.projectTwoRef.validate() ||
        !this.$refs.projectThreeRef.validate() ||
        !this.$refs.projectFourRef.validate()
      ) {
        return null;
      }

      // Request API
      let result = await api
        .put(
          `/api/v1/project/${this.tempProjectResource.id}`,
          this.tempProjectResource
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
        let index = this.listTempProject.findIndex(
          (x) => x.id == this.tempProjectResource.id
        );

        this.listTempProject[index] = result.resource;
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }

      this.showEditBtn = false;
      this.dialogToggle[2] = false;
    },
    async editProject(item) {
      let result = await this.getByGroupId(item.groupId);

      this.tempListGroup = [];
      this.tempListGroup.unshift(result);

      this.tempProjectResource = Object.assign({}, item);

      this.showEditBtn = true;
      this.dialogToggle[2] = true;
    },
    deleteProject(item) {
      this.tempDeleteProject = item;
      this.deleteToggle[2] = true;
    },
    async saveDeleteProject() {
      // Request API
      let result = await api
        .delete(`/api/v1/project/${this.tempDeleteProject.id}`)
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
        let index = this.listTempProject.findIndex(
          (x) => x.id == this.tempDeleteProject.id
        );

        this.listTempProject.splice(index, 1);
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }

      this.deleteToggle[2] = false;
    },
    async orderProjectNext(item) {
      let count = this.listTempProject.length;
      let index = this.listTempProject.findIndex((x) => x.id == item.id);

      if (index == count - 1) {
        let temp = this.listTempProject.slice(0, index);
        temp.unshift(this.listTempProject[index]);
        this.listTempProject = temp;
      } else {
        for (let i = 0; i < count; i++) {
          if (i == index) {
            let temp = this.listTempProject[i];
            this.listTempProject[i] = this.listTempProject[i + 1];
            this.listTempProject[i + 1] = temp;

            break;
          }
        }
      }

      let payload = [];
      this.listTempProject.forEach((x) => payload.push(x.id));

      // Request API
      let result = await api
        .put(`/api/v1/project`, payload)
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

      if (!result.success) {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async orderProjectPrev(item) {
      let count = this.listTempProject.length;
      let index = this.listTempProject.findIndex((x) => x.id == item.id);

      if (index == 0) {
        let temp = this.listTempProject.slice(1);
        temp.push(this.listTempProject[index]);
        this.listTempProject = temp;
      } else {
        for (let i = 0; i < count; i++) {
          if (i == index) {
            let temp = this.listTempProject[i];
            this.listTempProject[i] = this.listTempProject[i - 1];
            this.listTempProject[i - 1] = temp;

            break;
          }
        }
      }

      let payload = [];
      this.listTempProject.forEach((x) => payload.push(x.id));

      // Request API
      let result = await api
        .put(`/api/v1/project`, payload)
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

      if (!result.success) {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async orderWorkHistoryNext(item) {
      let count = this.listTempWorkHistory.length;
      let index = this.listTempWorkHistory.findIndex((x) => x.id == item.id);

      if (index == count - 1) {
        let temp = this.listTempWorkHistory.slice(0, index);
        temp.unshift(this.listTempWorkHistory[index]);
        this.listTempWorkHistory = temp;
      } else {
        for (let i = 0; i < count; i++) {
          if (i == index) {
            let temp = this.listTempWorkHistory[i];
            this.listTempWorkHistory[i] = this.listTempWorkHistory[i + 1];
            this.listTempWorkHistory[i + 1] = temp;

            break;
          }
        }
      }

      let payload = [];
      this.listTempWorkHistory.forEach((x) => payload.push(x.id));

      // Request API
      let result = await api
        .put(`/api/v1/work-history`, payload)
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

      if (!result.success) {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async orderWorkHistoryPrev(item) {
      let count = this.listTempWorkHistory.length;
      let index = this.listTempWorkHistory.findIndex((x) => x.id == item.id);

      if (index == 0) {
        let temp = this.listTempWorkHistory.slice(1);
        temp.push(this.listTempWorkHistory[index]);
        this.listTempWorkHistory = temp;
      } else {
        for (let i = 0; i < count; i++) {
          if (i == index) {
            let temp = this.listTempWorkHistory[i];
            this.listTempWorkHistory[i] = this.listTempWorkHistory[i - 1];
            this.listTempWorkHistory[i - 1] = temp;

            break;
          }
        }
      }

      let payload = [];
      this.listTempWorkHistory.forEach((x) => payload.push(x.id));

      // Request API
      let result = await api
        .put(`/api/v1/work-history`, payload)
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

      if (!result.success) {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async addWorkHistory() {
      if (
        !this.$refs.workHistoryOneRef.validate() ||
        !this.$refs.workHistoryTwoRef.validate() ||
        !this.$refs.workHistoryThreeRef.validate() ||
        !this.$refs.workHistoryFourRef.validate()
      ) {
        return null;
      }

      // Request API
      let result = await api
        .post(`/api/v1/work-history`, this.tempWorkHistoryResource)
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
        this.listTempWorkHistory.unshift(result.resource);
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }

      this.dialogToggle[3] = false;
    },
    async saveWorkHistory() {
      if (
        !this.$refs.workHistoryOneRef.validate() ||
        !this.$refs.workHistoryTwoRef.validate() ||
        !this.$refs.workHistoryThreeRef.validate() ||
        !this.$refs.workHistoryFourRef.validate()
      ) {
        return null;
      }

      // Request API
      let result = await api
        .put(
          `/api/v1/work-history/${this.tempWorkHistoryResource.id}`,
          this.tempWorkHistoryResource
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
        let index = this.listTempWorkHistory.findIndex(
          (x) => x.id == this.tempWorkHistoryResource.id
        );

        this.listTempWorkHistory[index] = result.resource;
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }

      this.showEditBtn = false;
      this.dialogToggle[3] = false;
    },
    editWorkHistory(item) {
      this.tempWorkHistoryResource = Object.assign({}, item);

      this.showEditBtn = true;
      this.dialogToggle[3] = true;
    },
    deleteWorkHistory(item) {
      this.tempDeleteWorkHistory = item;
      this.deleteToggle[3] = true;
    },
    async saveDeleteWorkHistory() {
      // Request API
      let result = await api
        .delete(`/api/v1/work-history/${this.tempDeleteWorkHistory.id}`)
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
        let index = this.listTempWorkHistory.findIndex(
          (x) => x.id == this.tempDeleteWorkHistory.id
        );

        this.listTempWorkHistory.splice(index, 1);
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }

      this.deleteToggle[3] = false;
    },
    async addEducation() {
      if (
        !this.$refs.educationOneRef.validate() ||
        !this.$refs.educationTwoRef.validate() ||
        !this.$refs.educationThreeRef.validate()
      ) {
        return null;
      }

      // Request API
      let result = await api
        .post(`/api/v1/education`, this.tempEducationResource)
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
        this.listTempEducation.unshift(result.resource);
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }

      this.dialogToggle[4] = false;
    },
    async saveEducation() {
      if (
        !this.$refs.educationOneRef.validate() ||
        !this.$refs.educationTwoRef.validate() ||
        !this.$refs.educationThreeRef.validate()
      ) {
        return null;
      }

      // Request API
      let result = await api
        .put(
          `/api/v1/education/${this.tempEducationResource.id}`,
          this.tempEducationResource
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
        let index = this.listTempEducation.findIndex(
          (x) => x.id == this.tempEducationResource.id
        );

        this.listTempEducation[index] = result.resource;
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }

      this.showEditBtn = false;
      this.dialogToggle[4] = false;
    },
    editEducation(item) {
      this.tempEducationResource = Object.assign({}, item);

      this.showEditBtn = true;
      this.dialogToggle[4] = true;
    },
    deleteEducation(item) {
      this.tempDeleteEducation = item;
      this.deleteToggle[4] = true;
    },
    async saveDeleteEducation() {
      // Request API
      let result = await api
        .delete(`/api/v1/education/${this.tempDeleteEducation.id}`)
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
        let index = this.listTempEducation.findIndex(
          (x) => x.id == this.tempDeleteEducation.id
        );

        this.listTempProject.splice(index, 1);
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }

      this.deleteToggle[4] = false;
    },
    async orderEducationNext(item) {
      let count = this.listTempEducation.length;
      let index = this.listTempEducation.findIndex((x) => x.id == item.id);

      if (index == count - 1) {
        let temp = this.listTempEducation.slice(0, index);
        temp.unshift(this.listTempEducation[index]);
        this.listTempEducation = temp;
      } else {
        for (let i = 0; i < count; i++) {
          if (i == index) {
            let temp = this.listTempEducation[i];
            this.listTempEducation[i] = this.listTempEducation[i + 1];
            this.listTempEducation[i + 1] = temp;

            break;
          }
        }
      }

      let payload = [];
      this.listTempEducation.forEach((x) => payload.push(x.id));

      // Request API
      let result = await api
        .put(`/api/v1/education`, payload)
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

      if (!result.success) {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async orderEducationPrev(item) {
      let count = this.listTempEducation.length;
      let index = this.listTempEducation.findIndex((x) => x.id == item.id);

      if (index == 0) {
        let temp = this.listTempEducation.slice(1);
        temp.push(this.listTempEducation[index]);
        this.listTempEducation = temp;
      } else {
        for (let i = 0; i < count; i++) {
          if (i == index) {
            let temp = this.listTempEducation[i];
            this.listTempEducation[i] = this.listTempEducation[i - 1];
            this.listTempEducation[i - 1] = temp;

            break;
          }
        }
      }

      let payload = [];
      this.listTempEducation.forEach((x) => payload.push(x.id));

      // Request API
      let result = await api
        .put(`/api/v1/education`, payload)
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

      if (!result.success) {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async addCertificate() {
      if (
        !this.$refs.certificateOneRef.validate() ||
        !this.$refs.certificateTwoRef.validate() ||
        !this.$refs.certificateThreeRef.validate()
      ) {
        return null;
      }

      // Request API
      let result = await api
        .post(`/api/v1/certificate`, this.tempCertificateResource)
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
        this.listTempCertificate.unshift(result.resource);
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }

      this.dialogToggle[5] = false;
    },
    async saveCertificate() {
      if (
        !this.$refs.certificateOneRef.validate() ||
        !this.$refs.certificateTwoRef.validate() ||
        !this.$refs.certificateThreeRef.validate()
      ) {
        return null;
      }

      // Request API
      let result = await api
        .put(
          `/api/v1/certificate/${this.tempCertificateResource.id}`,
          this.tempCertificateResource
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
        let index = this.listTempCertificate.findIndex(
          (x) => x.id == this.tempCertificateResource.id
        );

        this.listTempCertificate[index] = result.resource;
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }

      this.showEditBtn = false;
      this.dialogToggle[5] = false;
    },
    editCertificate(item) {
      this.tempCertificateResource = Object.assign({}, item);

      this.showEditBtn = true;
      this.dialogToggle[5] = true;
    },
    deleteCertificate(item) {
      this.tempDeleteCertificate = item;
      this.deleteToggle[5] = true;
    },
    async saveDeleteCertificate() {
      // Request API
      let result = await api
        .delete(`/api/v1/certificate/${this.tempDeleteCertificate.id}`)
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
        let index = this.listTempCertificate.findIndex(
          (x) => x.id == this.tempDeleteCertificate.id
        );

        this.listTempCertificate.splice(index, 1);
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }

      this.deleteToggle[5] = false;
    },
    async orderCertificateNext(item) {
      let count = this.listTempCertificate.length;
      let index = this.listTempCertificate.findIndex(
        (x) => x.tempId == item.tempId
      );

      if (index == count - 1) {
        let temp = this.listTempCertificate.slice(0, index);
        temp.unshift(this.listTempCertificate[index]);
        this.listTempCertificate = temp;
      } else {
        for (let i = 0; i < count; i++) {
          if (i == index) {
            let temp = this.listTempCertificate[i];
            this.listTempCertificate[i] = this.listTempCertificate[i + 1];
            this.listTempCertificate[i + 1] = temp;

            break;
          }
        }
      }

      let payload = [];
      this.listTempCertificate.forEach((x) => payload.push(x.id));

      // Request API
      let result = await api
        .put(`/api/v1/certificate`, payload)
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

      if (!result.success) {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async orderCertificatePrev(item) {
      let count = this.listTempCertificate.length;
      let index = this.listTempCertificate.findIndex((x) => x.id == item.id);

      if (index == 0) {
        let temp = this.listTempCertificate.slice(1);
        temp.push(this.listTempCertificate[index]);
        this.listTempCertificate = temp;
      } else {
        for (let i = 0; i < count; i++) {
          if (i == index) {
            let temp = this.listTempCertificate[i];
            this.listTempCertificate[i] = this.listTempCertificate[i - 1];
            this.listTempCertificate[i - 1] = temp;

            break;
          }
        }
      }

      let payload = [];
      this.listTempCertificate.forEach((x) => payload.push(x.id));

      // Request API
      let result = await api
        .put(`/api/v1/certificate`, payload)
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

      if (!result.success) {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async requestUpdateEmployee() {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      // Request API
      let result = await api
        .put(`/api/v1/person/${this.employeeInfor.id}`, this.employeeInfor)
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
        return result;
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });

        return result;
      }
    },
    async requestUpdateImage() {
      if (!this.imageFile) return null;

      let fd = new FormData();
      fd.append("image", this.imageFile);

      // Request API update image
      return api
        .put(`/api/v1/person/image/${this.employeeInfor.id}`, fd)
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
    },
    async saveAll() {
      try {
        this.loadingSave = true;

        let result = await Promise.all([
          this.requestUpdateEmployee(),
          this.requestUpdateImage(),
        ]);

        if (result[0]?.success) {
          this.$q.notify({
            type: "positive",
            message: "Successfully updated",
          });

          this.$emit("updateSuccess", true);
        } else {
          this.$q.notify({
            type: "negative",
            message: result[0]?.message[0],
          });
        }
      } finally {
        this.loadingSave = false;
      }
    },
    async resetAvatar() {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      // Request API
      let result = await api
        .put(`/api/v1/person/reset-avatar/${this.employeeInfor.id}`)
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
        this.imageFile = null;
        this.imageURL = result.resource.avatar.original;
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    validateDoB(dateTarget) {
      if (!date.isValid(dateTarget)) return false;

      let now = new Date(Date.now());

      let dateFrom = date.subtractFromDate(now, { years: 100 });
      let dateTo = date.addToDate(now, { years: 15 });

      return date.isBetweenDates(dateTarget, dateFrom, dateTo, {
        onlyDate: true,
      });
    },
    validateDate(dateTarget) {
      return date.isValid(dateTarget);
    },
    convertDateTimeToDate(dateTime) {
      return date.formatDate(dateTime, "YYYY-MM-DD");
    },
    mappingDataUpdate() {
      // Set infor
      extend(true, this.employeeInfor, this.employeeTransfer);
      // Set position
      this.employeeInfor.positionId = this.employeeInfor?.position?.id;
      // Set self-project
      this.employeeInfor.groupId = this.employeeInfor?.group?.id;
      this.tempListWork.push(this.employeeInfor.group);
      // Set image
      this.imageURL = this.employeeInfor.avatar.original;
      // Skill component
      this.listTempCategory = this.employeeInfor.categoryPerson;
      this.tempCategoryPersonResource.personId = this.employeeInfor.id;
      // Project component
      this.listTempProject = this.employeeInfor.project;
      this.tempProjectResource.personId = this.employeeInfor.id;
      // Work-History component
      this.listTempWorkHistory = this.employeeInfor.workHistory;
      this.tempWorkHistoryResource.personId = this.employeeInfor.id;
      // Education component
      this.listTempEducation = this.employeeInfor.education;
      this.tempEducationResource.personId = this.employeeInfor.id;
      // Certificate component
      this.listTempCertificate = this.employeeInfor.certificate;
      this.tempCertificateResource.personId = this.employeeInfor.id;
      // Order Component
      let tempTab = [];

      for (let i = 0; i < this.employeeInfor.orderIndex.length; i++) {
        let element = this.employeeInfor.orderIndex[i];
        let temp = this.tabModel.find((x) => parseInt(x.id) == element);

        if (temp) tempTab.push(temp);
      }

      for (let i = 0; i < this.tabModel.length; i++) {
        let element = this.tabModel[i];
        let isSuccess = this.employeeInfor.orderIndex.includes(
          parseInt(element.id)
        );

        if (!isSuccess) tempTab.push(element);
      }

      this.tabModel = tempTab;
      this.tab = this.tabModel[0].id;
    },
  },
  computed: {
    ...mapGetters("auth", ["getInformation"]),
    getDisableCategory() {
      let temp = [];
      this.listTempCategory.forEach((x) => temp.push(x.categoryId));

      return temp;
    },
  },
  async created() {
    this.mappingDataUpdate();

    let isValid = await this.validateToken();
    if (!isValid) this.$router.replace("/login");

    await Promise.all([
      this.findCategory(false, true),
      this.findGroup(false, true),
      this.getPosition(),
      this.getAvatarUrl(),
    ]);
  },
  watch: {
    statusUpdateTransfer: {
      immediate: true,
      deep: true,
      async handler(newValue, oldValue) {
        if (newValue) {
          await this.saveAll();
        }
      },
    },
  },
  mounted() {
    const $q = useQuasar();
  },
});
</script>

<style lang="scss" scoped>
.container {
  min-height: inherit;
  height: 100%;
  width: 100%;
}

.inside {
  margin: 16px;
  &:last-child {
    margin: 16px 16px 0 16px;
  }
  &:first-child {
    margin: 0 16px 16px 16px;
  }
}
</style>
