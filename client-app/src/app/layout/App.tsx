import React, { Fragment, useEffect, useState } from "react";
import axios from "axios";
import "../layout/styles.css";
import { Container, List } from "semantic-ui-react";
import { Activity } from "../models/activity";
import NavBar from "./NavBar";
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";
// import { v4 as uuid} from 'uudi'


function App() {
  const [activities, setActivities] = useState<Activity[]>([]);
  const [selectedActivity, setSelectedActivity] = useState<Activity | undefined>(undefined);
  const [editMode, setEditMode] = useState(false)

  useEffect(() => {
    axios
      .get<Activity[]>("http://localhost:5000/api/activities")
      .then((response) => {
        setActivities(response.data);
      });
  }, []);

  function handleSelectActivity(id: string){
    setSelectedActivity(activities.find(a => a.id === id))
  }

  function handleCancelSelectActivity(){
    setSelectedActivity(undefined);
  }

  function handleFormOpen(id?: string){
    id ? handleSelectActivity(id) : handleCancelSelectActivity();
    setEditMode(true);
  }

  function handleFormClose() {
    setEditMode(false);
  }

  function handleCreateOrEditActivity(activity: Activity){
    activity.id 
      ? setActivities([activity,...activities.filter(a => a.id !== activity.id)])
      : setActivities([...activities,activity]);
    setEditMode(false);
    setSelectedActivity(activity);
  }

  return (
    <Fragment>
      <NavBar openForm={handleFormOpen} />
      <Container style={{margin: '7em'}}>
        <ActivityDashboard 
          activities={activities}
          selectedActivity={selectedActivity}
          selectActivity={handleSelectActivity}
          cancelSelectActivity={handleCancelSelectActivity}
          editMode={editMode}
          openForm={handleFormOpen}
          closeForm={handleFormClose}
          createOrEdit={handleCreateOrEditActivity} />
      </Container>
    </Fragment>
  );
}

export default App;