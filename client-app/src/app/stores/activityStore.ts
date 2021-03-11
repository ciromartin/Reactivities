import { format } from 'date-fns';
import { makeAutoObservable, toJS } from 'mobx'
import agent from '../api/agent';
import { Activity } from '../models/activity'

export default class ActivityStore {
    activityRegistry = new Map<string, Activity>();
    selectedActivity: Activity | undefined = undefined;
    editMode = false;
    loading = false;
    loadingInitial = false;

    constructor() {
        makeAutoObservable(this)
    }

    get activitiesOrderedByDate() {
        return Array.from(this.activityRegistry.values()).sort((a, b) => a.date!.getTime() - b.date!.getTime());
    }

    get groupedByDateActivities() {
        return Object.entries(            
            this.activitiesOrderedByDate.reduce((activities, activity)=> {
                const date = format(activity.date!, 'dd MMM yyyy');
                activities[date] = activities[date] ? [...activities[date], activity] : [activity];
                return activities;
            }, {} as {[key: string]: Activity[]})
        )
    }

    loadActivities = async () => {
        this.setLoadingInitial(true);
        try {
            const activities = await agent.Activities.list();
            activities.forEach((activity) => {
                this.setOrAddActivity(activity);
            });
            this.setLoadingInitial(false);
        } catch (error) {
            console.error(error);
            this.setLoadingInitial(false);
        }
    }

    loadActivity = async (id: string) => {
        let activity = this.getActivity(id);
        if (activity) {
            this.setOrAddActivity(activity);
            this.setSelectedActivity(activity);
            return activity;
        }
        else {
            this.setLoadingInitial(true);
            try {
                const respActivity = await agent.Activities.details(id);
                this.setSelectedActivity(respActivity);
                this.setOrAddActivity(respActivity);
                this.setLoadingInitial(false);
                return this.getActivity(id);
            } catch (error) {
                console.error(error);
                this.setLoadingInitial(false);
            }
        }

    }

    private getActivity = (id: string) => {
        return toJS(this.activityRegistry.get(id));
    }

    setLoadingInitial = (state: boolean) => {
        this.loadingInitial = state;
    }

    setEditMode = (state: boolean) => {
        this.editMode = state;
    }

    setLoading = (state: boolean) => {
        this.loading = state;
    }

    setSelectedActivity = (activity: Activity) => {
        this.selectedActivity = this.setActivityDate(activity);
    }

    private setActivityDate = (activity: Activity) => {
        activity.date = new Date(activity.date!);
        return activity;
    }

    setOrAddActivity = (activity: Activity) => {
        this.activityRegistry.set(activity.id, this.setActivityDate(activity));
    }

    getActivities = () => {
        return toJS(this.activityRegistry);
    }

    createActivity = async (activity: Activity) => {
        this.setLoading(true);
        try {
            const response = await agent.Activities.create(activity);
            activity.id = response;
            this.setOrAddActivity(activity);
            this.setSelectedActivity(activity);
            this.setEditMode(false);
            this.setLoading(false);
            return activity.id;
        } catch (error) {
            console.error(error);
            this.setLoading(false);
        }
    }

    updateActivity = async (activity: Activity) => {
        this.setLoading(true);
        try {
            await agent.Activities.update(activity);
            this.setOrAddActivity(activity);
            this.setSelectedActivity(activity);
            this.setEditMode(false);
            this.setLoading(false);
        } catch (error) {
            console.error(error);
            this.setLoading(false);
        }
    }

    deleteActivity = async (id: string) => {
        this.setLoading(true);
        try {
            await agent.Activities.delete(id);
            this.activityRegistry.delete(id);
            this.setLoading(false);
        } catch (error) {
            console.error(error);
            this.setLoading(false);
        }
    }

}