import { ErrorMessage, Form, Formik, FormikProvider } from "formik";
import { observer } from "mobx-react-lite";
import React, { ChangeEvent, useEffect, useState } from "react";
import { Link, useHistory, useParams } from "react-router-dom";
import { Button, Header, Label, Segment } from "semantic-ui-react";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import { useStore } from "../../../app/stores/store";
import TextInputCustom from "../../../app/common/form/TextInputCustom"
import TextAreaCustom from "../../../app/common/form/TextAreaCustom"
import SelectInputCustom from "../../../app/common/form/SelectInputCustom"
import DateInputCustom from "../../../app/common/form/DateInputCustom"
import {categoryOptions} from "../../../app/common/options/categoryOptions"
import * as Yup from 'yup'
import { Activity } from "../../../app/models/activity";
import { dir } from "node:console";

export default observer(function ActivityForm() {
  const history = useHistory();
  const {activityStore} = useStore();
  const {createActivity, updateActivity, loading, loadActivity, loadingInitial} = activityStore;
  const {id} = useParams<{id:string}>();

  
  const[activity, setActivity] = useState<Activity>({
    id: "",
    title: "",
    date: null,
    description: "",
    category: "",
    city: "",
    venue: "",
  });

  const validationSchema = Yup.object({
    title: Yup.string().required(),
    description: Yup.string().required(),
    category: Yup.string().required(),
    city: Yup.string().required(),
    venue: Yup.string().required(),
    date: Yup.string().required().nullable()
  })

  useEffect(() => {
    if(id) loadActivity(id).then(activity => {
      setActivity(activity!)
    })
  }, [id, loadActivity])

  function handleFormSubmit(activity: Activity) {
      activity.id ? updateActivity(activity).then(() => { history.push(`/activities/${activity.id}`) }) : createActivity(activity).then(id => { history.push(`/activities/${id}`) });
  }


  if (loadingInitial) return <LoadingComponent content='Loading component...'/>

  return (
    <Segment clearing>
      <Header content='Activity Details' sub color='teal' />
      <Formik 
          validationSchema={validationSchema}
          enableReinitialize 
          initialValues={activity} 
          onSubmit={values => handleFormSubmit(values)}>
        {({handleSubmit, isValid, isSubmitting, dirty}) => (
          <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
            <TextInputCustom placeholder="Title" name='title' />            
            <TextAreaCustom placeholder="Description" name='description' rows={4}  />
            <SelectInputCustom placeholder="Category" name='category' options={categoryOptions}  />
            <DateInputCustom 
              placeholderText="Date" 
              name='date' 
              showTimeSelect
              timeCaption='time'
              dateFormat='MMMM d, yyyy h:mm aa'
            />
            <Header content='Location Details' sub color='teal' />
            <TextInputCustom placeholder="City" name='city' />
            <TextInputCustom placeholder="Venue" name='venue'  />
            <Button 
              disabled={isSubmitting || !dirty || !isValid}
              loading={loading}
              floated="right" 
              positive 
              type="submit" 
              content="Submit" />
            <Button
              as={Link}
              to='/activities'
              floated="right"
              type="button"
              content="Cancel"
            />
          </Form>
        )}
      </Formik>
      
    </Segment>
  );
})
