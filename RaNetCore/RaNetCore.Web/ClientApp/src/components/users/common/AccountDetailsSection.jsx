import React from 'react'
import PropTypes from 'prop-types'
import classNames from 'classnames'
import { makeStyles } from '@material-ui/styles'
import Box from '@material-ui/core/Box'
import Card from '@material-ui/core/Card'
import CardHeader from '@material-ui/core/CardHeader'
import Divider from '@material-ui/core/Divider'
import EditIcon from '@material-ui/icons/Edit'

const useStyles = makeStyles((theme) => ({
    root: {
        marginBottom: theme.spacing(2)
    },
}))

const Title = ({ title }) => (
    <Box display="flex" alignItems="center">
        <Box mr="10px"> {title} </Box>
        <EditIcon />
    </Box>
)

const AccountDetailsSection = ({ title, className, children }) => {
    const classes = useStyles()

    return (
        <Card className={classNames(classes.root, className)}>
            <CardHeader
                // subheader="The information can be edited"
                title={<Title title={title} />}
            />
            <Divider />
            {children}
        </Card>
    )
}

AccountDetailsSection.propTypes = {
    className: PropTypes.string,
    children: PropTypes.any,
}

export default AccountDetailsSection
