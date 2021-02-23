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
        try {
            const activities = await agent.Activities.list();
            activities.forEach((activity) => {
                activity.date = activity.date.split("T")[0];
                this.addActivity(activity);
            });
            this.setLoadingInitial(false);
        } catch (error) {
            console.error(error);
            this.setLoadingInitial(false);
        }
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

    addActivity = (activity: Activity) => {
        this.activityRegistry.set(activity.id, activity);
    }

    getActivities = () => {
        return toJS(this.activityRegistry);
    }

    selectActivity = (id: string) => {
        this.selectedActivity = this.activityRegistry.get(id);
    }

    cancelSelectedActivity = () => {
        this.selectedActivity = undefined;
    }

    openForm = (id?: string) => {
        id ? this.selectActivity(id) : this.cancelSelectedActivity();
        this.setEditMode(true);
    }

    closeForm = () => {
        this.setEditMode(false);
    }

    createActivity = async (activity: Activity) => {
        this.setLoading(true);
        try {
            const response = await agent.Activities.create(activity);
            activity.id = response;
            this.addActivity(activity);
            this.selectActivity(activity.id);
            this.setEditMode(false);
            this.setLoading(false);
        } catch (error) {
            console.error(error);
            this.setLoading(false);
        }
    }

    updateActivity = async (activity: Activity) => {
        this.setLoading(true);
        try {
            await agent.Activities.update(activity);
            this.activityRegistry.set(activity.id, activity);
            this.selectActivity(activity.id);
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
            if(this.selectedActivity?.id === id) this.cancelSelectedActivity();
            this.setLoading(false);
        } catch (error) {
            console.error(error);
            this.setLoading(false);
        }
    }

}