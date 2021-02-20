import React, { Fragment, useEffect, useState } from "react";
import "../layout/styles.css";
import { Container, List } from "semantic-ui-react";
import { Activity } from "../models/activity";
import NavBar from "./NavBar";
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";
import { v4 as uuid } from "uuid";
import agent from "../api/agent";
import LoadingComponent from '../layout/LoadingComponent'

function App() {
  const [activities, setActivities] = useState<Activity[]>([]);
  const [selectedActivity, setSelectedActivity] = useState<Activity | undefined>(undefined);
  const [editMode, setEditMode] = useState(false);
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);

  useEffect(() => {
    agent.Activities.list().then((response) => {
      let activities: Activity[] = [];
      response.forEach((activity) => {
        activity.date = activity.date.split("T")[0];
        activities.push(activity);
      });
      setActivities(activities);
      setLoading(false);
    });
  }, []);

  function handleSelectActivity(id: string) {
    setSelectedActivity(activities.find((a) => a.id === id));
  }

  function handleCancelSelectActivity() {
    setSelectedActivity(undefined);
  }

  function handleFormOpen(id?: string) {
    id ? handleSelectActivity(id) : handleCancelSelectActivity();
    setEditMode(true);
  }

  function handleFormClose() {
    setEditMode(false);
  }

  function handleCreateOrEditActivity(activity: Activity) {
    setSubmitting(true);
    if(activity.id) {
      agent.Activities.update(activity).then(() => {
        setActivities([
          ...activities.filter((a) => a.id !== activity.id),
          activity
        ])
        setSelectedActivity(activity);
        setEditMode(false);
        setSubmitting(false);
      })
    }
    else{
      agent.Activities.create(activity).then(response => {
        setActivities([...activities,  { ...activity, id: response }]);
      })
      setSelectedActivity(activity);
      setEditMode(false);
      setSubmitting(false);
    }
  }

  function handleDeleteActivity(id: string) {
    setActivities([...activities.filter((a) => a.id !== id)]);
  }

  if (loading) return <LoadingComponent content='Loading app'/>

  return (
    <Fragment>
      <NavBar openForm={handleFormOpen} />
      <Container style={{ margin: "7em" }}>
        <ActivityDashboard
          activities={activities}
          selectedActivity={selectedActivity}
          selectActivity={handleSelectActivity}
          cancelSelectActivity={handleCancelSelectActivity}
          editMode={editMode}
          openForm={handleFormOpen}
          closeForm={handleFormClose}
          createOrEdit={handleCreateOrEditActivity}
          deleteActivity={handleDeleteActivity}
          submitting={submitting}
        />
      </Container>
    </Fragment>
  );
}

export default App;
