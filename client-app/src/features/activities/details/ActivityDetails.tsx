import { observer } from "mobx-react-lite";
import React, { useEffect } from "react";
import { useParams } from "react-router-dom";
import { Grid } from "semantic-ui-react";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import { useStore } from "../../../app/stores/store";
import ActivityDetailedHeader from './ActivityDetailedHeader'
import ActivityDetailedChat from './ActivityDetailedChat'
import ActivityDetailedInfo from './ActivityDetailedInfo'
import ActivityDetailedSideBar from './ActivityDetailedSideBar'

export default observer(function ActivityDetail() {

  const {activityStore} = useStore();
  const {selectedActivity: activity, loadActivity, loadingInitial} = activityStore
  const {id} = useParams<{id: string}>();

  useEffect(() => {
    if (id) loadActivity(id);
  }, [id, loadActivity])


  if (loadingInitial || !activity?.id) return <LoadingComponent/>;
  
  return (
    <Grid>
      <Grid.Column width={10}>
        <ActivityDetailedHeader activity={activity} />
        <ActivityDetailedInfo activity={activity} />
        <ActivityDetailedChat />
      </Grid.Column>
      <Grid.Column width={6}>
        <ActivityDetailedSideBar/>
      </Grid.Column>
    </Grid>
  );
})
