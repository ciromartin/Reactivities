import { makeAutoObservable, toJS } from 'mobx'
import agent from '../api/agent';
import { Activity } from '../models/activity'

export default class ActivityStore {
    activityRegistry = new Map<string, Activity>();
    selectedActivity: Activity | undefined = undefined;
    editMode = false;
    loading = false;
    loadingInitial = true;

    constructor() {
        makeAutoObservable(this)
    }

    get activitiesByDate() {
        return Array.from(this.activityRegistry.values()).sort((a, b) => Date.parse(a.date) - Date.parse(b.date));
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
                console.log(error);
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
        this.selectedActivity = activity;
    }

    private setActivityDate = (activity: Activity) => {
        activity.date = activity.date.split("T")[0];
        return activity;
    }

    setOrAddActivity = (activity: Activity) => {
        activity = this.setActivityDate(activity);
        this.activityRegistry.set(activity.id, activity);
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