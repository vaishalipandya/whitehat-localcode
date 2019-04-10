using System;
using EnvDTE;
using EnvDTE80;
using System.Collections.Generic;
using WhiteHatSec.Entity;
using System.Collections;

namespace WhiteHatSec.VSIX.Utility
{
    /// <summary>
    ///     Get Solution Information.
    /// </summary>
    public static class SolutionInfo
    {
        /// <summary>
        ///     Finds Solution item by name.
        /// </summary>
        /// <param name="currentVisualStudio">The current instance of Visual studio.</param>
        /// <param name="name">The name.</param>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        /// <returns></returns>
        public static ProjectItem FindSolutionItemByName(DTE2 currentVisualStudio, string name, bool recursive)
        {
            ProjectItem projectItem = null;
            foreach (Project project in currentVisualStudio.Solution.Projects)
            {
                projectItem = FindProjectItemInProject(project, name, recursive);

                if (projectItem != null)
                {
                    break;
                }
            }

            return projectItem;
        }

        /// <summary>
        ///     Finds the project item in the project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="name">The name.</param>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        /// <returns></returns>
        public static ProjectItem FindProjectItemInProject(Project project, string name, bool recursive)
        {
            ProjectItem projectItem = null;

            if (project.Kind != Constants.vsProjectKindSolutionItems)
            {
                if (project.ProjectItems != null && project.ProjectItems.Count > 0)
                {
                    projectItem = FindItemByName(project.ProjectItems, name, recursive);
                }
            }
            else
            {
                // if solution folder, one of its ProjectItems might be a real project
                foreach (ProjectItem item in project.ProjectItems)
                {
                    Project realProject = item.Object as Project;

                    if (realProject != null)
                    {
                        projectItem = FindProjectItemInProject(realProject, name, recursive);

                        if (projectItem != null)
                        {
                            break;
                        }
                    }
                }
            }

            return projectItem;
        }

        /// <summary>
        ///     Finds the project item by name.
        /// </summary>
        /// <param name="projectCollection">The collection of project Item.</param>
        /// <param name="name">The name.</param>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        /// <returns></returns>
        public static ProjectItem FindItemByName(ProjectItems projectCollection, string name, bool recursive)
        {
            if (projectCollection != null)
            {
                foreach (ProjectItem project in projectCollection)
                {
                    if (project.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return project;
                    }

                    if (recursive)
                    {
                        ProjectItem projectItemName = FindItemByName(project.ProjectItems, name, true);
                        if (projectItemName != null)
                        {
                            return projectItemName;
                        }
                    }
                }
            }

            return null;
        }
      
        /// <summary>
        ///     Gets the solution folder projects.
        /// </summary>
        /// <param name="solutionFolderProjects">The solution folder.</param>
        /// <returns></returns>
        public static IEnumerable<Project> GetSolutionFolderProjects(Project solutionFolderProjects)
        {
            List<Project> projects = new List<Project>();
            for (int i = 1; i <= solutionFolderProjects.ProjectItems.Count; i++)
            {
                Project subProject = solutionFolderProjects.ProjectItems.Item(i).SubProject;
                if (subProject == null)
                {
                    continue;
                }

                // If this is another solution folder, do a recursive call, otherwise add
                if (subProject.Kind == ProjectKinds.vsProjectKindSolutionFolder)
                {
                    projects.AddRange(GetSolutionFolderProjects(subProject));
                }
                else
                {
                    projects.Add(subProject);
                }
            }

            return projects;
        }

        /// <summary>
        ///     Gets the projects.
        /// </summary>
        /// <param name="visualStudioCurrentInstance">The application object.</param>
        /// <returns></returns>
        public static IList<ProjectItemsInfo> GetProjects(DTE2 visualStudioCurrentInstance)
        {
            Projects solutionProjects = visualStudioCurrentInstance.Solution.Projects;
            List<Project> projects = new List<Project>();
            List<ProjectItemsInfo> projectItemInfo = new List<ProjectItemsInfo>();
           IEnumerator solutionProjectItem = solutionProjects.GetEnumerator();
            while (solutionProjectItem.MoveNext())
            {
                Project project = solutionProjectItem.Current as Project;
                if (project == null)
                {
                    continue;
                }

                if (project.Kind == ProjectKinds.vsProjectKindSolutionFolder)
                {
                    projects.AddRange(GetSolutionFolderProjects(project));
                }
                else
                {
                    if (project.Name == "Miscellaneous Files") continue;
                    projects.Add(project);
                    ProjectItemsInfo projectItems = new ProjectItemsInfo
                    {
                        Id = project.FullName,
                        Name = project.Name,
                        ProjectPath = project.FullName
                    };
                    projectItemInfo.Add(projectItems);
                }
            }

            return projectItemInfo;
        }
    }
}