import React from 'react'
import { Redirect, useLocation } from 'react-router-dom'
import { ChipFieldArray, Show, SplitShowContainer, TextField } from '../_design'
import { SimpleShowLayout, Tab, TabbedShowLayout } from 'react-admin'
import UserProfileAvatar from './common/UserProfileAvatar'
import UserTitle from './common/UserTitle'

const UserShow = (props) => {
    const location = useLocation()

    if (location.pathname.endsWith('show')) {
        return <Redirect to={`${location.pathname}/activities`} />
    }

    return (
        <Show {...props} component="div" title={<UserTitle />}>
            <SimpleShowLayout>
                <SplitShowContainer
                    summaryView={
                        <SimpleShowLayout>
                            <UserProfileAvatar />
                            <TextField source="fullName" />
                            <TextField source="email" />
                            <ChipFieldArray source="roles" />
                        </SimpleShowLayout>
                    }
                    detailsView={
                        <TabbedShowLayout>
                            <Tab label="Activities" path="activities">
                                {/* <UserActivitiesTab /> */}
                            </Tab>
                            <Tab label="Daily Costs" path="daily-costs">
                                {/* <UserDailyCostsTab /> */}
                            </Tab>
                        </TabbedShowLayout>
                    }
                />
            </SimpleShowLayout>
        </Show>
    )
}

export default UserShow
