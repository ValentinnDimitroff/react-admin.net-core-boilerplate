import React from 'react'
import { useAuthProvider } from 'react-admin'
import Card from '@material-ui/core/Card'
import CardHeader from '@material-ui/core/CardHeader'

const Dashboard = () => {
    const authProvider = useAuthProvider()
    const currentUser = authProvider.getCurrentUser()
    return (
        <Card>
            <CardHeader title={`Welcome, ${currentUser.firstName} ${currentUser.lastName}!`} />
        </Card>
    )
}

export default Dashboard
